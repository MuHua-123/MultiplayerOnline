using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 通用在线发现基类，支持局域网/在线服务器发现与响应
/// </summary>
[DisallowMultipleComponent]
public abstract class OnlineDiscovery<TBroadCast, TResponse> : MonoBehaviour
	where TBroadCast : INetworkSerializable, new()
	where TResponse : INetworkSerializable, new() {
	/// <summary>
	/// 消息类型枚举
	/// </summary>
	private enum MessageType : byte {
		BroadCast = 0,
		Response = 1,
	}

	private UdpClient m_Client;

	[SerializeField]
	private ushort m_Port = 47777;

	// 唯一应用ID，用于区分不同项目的发现包
	[SerializeField]
	private long m_UniqueApplicationId;

	private CancellationTokenSource m_CancellationTokenSource;

	/// <summary> 是否正在运行 </summary>
	public bool IsRunning { get; private set; }
	/// <summary> 是否为服务器模式 </summary>
	public bool IsServer { get; private set; }
	/// <summary> 是否为客户端模式 </summary>
	public bool IsClient { get; private set; }

	/// <summary>
	/// 验证唯一应用ID，若为0则自动生成
	/// </summary>
	private void OnValidate() {
		if (m_UniqueApplicationId == 0) {
			var value1 = (long)Random.Range(int.MinValue, int.MaxValue);
			var value2 = (long)Random.Range(int.MinValue, int.MaxValue);
			m_UniqueApplicationId = value1 + (value2 << 32);
		}
	}

	/// <summary>
	/// 客户端广播发现请求
	/// </summary>
	/// <param name="broadCast">广播数据</param>
	public void ClientBroadcast(TBroadCast broadCast) {
		if (!IsClient) { throw new InvalidOperationException("Cannot send client broadcast while not running in client mode. Call StartClient first."); }

		IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, m_Port);

		using (FastBufferWriter writer = new FastBufferWriter(1024, Allocator.Temp, 1024 * 64)) {
			WriteHeader(writer, MessageType.BroadCast);
			writer.WriteNetworkSerializable(broadCast);
			var data = writer.ToArray();

			try {
				m_Client.SendAsync(data, data.Length, endPoint);
			}
			catch (Exception e) {
				Debug.LogError(e);
			}
		}
	}

	/// <summary>
	/// 启动服务器模式，监听客户端广播并响应
	/// </summary>
	public virtual void StartServer() => StartDiscovery(true);

	/// <summary>
	/// 启动客户端模式，监听服务器响应
	/// </summary>
	public virtual void StartClient() => StartDiscovery(false);

	/// <summary>
	/// 停止发现服务，关闭UDP和任务
	/// </summary>
	public virtual void StopDiscovery() {
		IsClient = false;
		IsServer = false;
		IsRunning = false;

		m_CancellationTokenSource?.Cancel();
		m_CancellationTokenSource = null;

		if (m_Client != null) {
			try { m_Client.Close(); }
			catch (Exception) { /* 忽略关闭异常 */ }
			m_Client = null;
		}
	}

	/// <summary>
	/// 处理收到的广播，生成响应数据
	/// </summary>
	/// <param name="sender">广播发送方</param>
	/// <param name="broadCast">广播内容</param>
	/// <param name="response">响应内容</param>
	/// <returns>是否需要回复</returns>
	protected abstract bool ProcessBroadcast(IPEndPoint sender, TBroadCast broadCast, out TResponse response);

	/// <summary>
	/// 处理收到的响应
	/// </summary>
	/// <param name="sender">响应发送方</param>
	/// <param name="response">响应内容</param>
	protected abstract void ResponseReceived(IPEndPoint sender, TResponse response);

	/// <summary>
	/// 启动发现服务（服务器或客户端模式）
	/// </summary>
	/// <param name="isServer">是否为服务器</param>
	private void StartDiscovery(bool isServer) {
		StopDiscovery();

		IsServer = isServer;
		IsClient = !isServer;

		// 服务器监听指定端口，客户端随机端口
		var port = isServer ? m_Port : 0;
		m_Client = new UdpClient(port) { EnableBroadcast = true, MulticastLoopback = false };

		m_CancellationTokenSource = new CancellationTokenSource();
		_ = ListenAsync(isServer ? ReceiveBroadcastAsync : new Func<Task>(ReceiveResponseAsync), m_CancellationTokenSource.Token);

		IsRunning = true;
	}

	/// <summary>
	/// 持续异步监听UDP消息
	/// </summary>
	private async Task ListenAsync(Func<Task> onReceiveTask, CancellationToken token) {
		while (!token.IsCancellationRequested) {
			try {
				await onReceiveTask();
			}
			catch (ObjectDisposedException) {
				// socket已关闭，退出循环
				break;
			}
			catch (Exception e) {
				Debug.LogException(e);
			}
		}
	}

	/// <summary>
	/// 客户端异步接收服务器响应
	/// </summary>
	private async Task ReceiveResponseAsync() {
		UdpReceiveResult udpReceiveResult = await m_Client.ReceiveAsync();
		var segment = new ArraySegment<byte>(udpReceiveResult.Buffer, 0, udpReceiveResult.Buffer.Length);
		using var reader = new FastBufferReader(segment, Allocator.Persistent);

		try {
			if (!ReadAndCheckHeader(reader, MessageType.Response)) { return; }

			reader.ReadNetworkSerializable(out TResponse receivedResponse);
			ResponseReceived(udpReceiveResult.RemoteEndPoint, receivedResponse);
		}
		catch (Exception e) {
			Debug.LogException(e);
		}
	}

	/// <summary>
	/// 服务器异步接收客户端广播并响应
	/// </summary>
	private async Task ReceiveBroadcastAsync() {
		UdpReceiveResult udpReceiveResult = await m_Client.ReceiveAsync();
		var segment = new ArraySegment<byte>(udpReceiveResult.Buffer, 0, udpReceiveResult.Buffer.Length);
		using var reader = new FastBufferReader(segment, Allocator.Persistent);

		try {
			if (!ReadAndCheckHeader(reader, MessageType.BroadCast)) { return; }

			reader.ReadNetworkSerializable(out TBroadCast receivedBroadcast);

			if (ProcessBroadcast(udpReceiveResult.RemoteEndPoint, receivedBroadcast, out TResponse response)) {
				using var writer = new FastBufferWriter(1024, Allocator.Persistent, 1024 * 64);
				WriteHeader(writer, MessageType.Response);
				writer.WriteNetworkSerializable(response);
				var data = writer.ToArray();

				await m_Client.SendAsync(data, data.Length, udpReceiveResult.RemoteEndPoint);
			}
		}
		catch (Exception e) {
			Debug.LogException(e);
		}
	}

	/// <summary>
	/// 写入包头（唯一应用ID和消息类型）
	/// </summary>
	private void WriteHeader(FastBufferWriter writer, MessageType messageType) {
		writer.WriteValueSafe(m_UniqueApplicationId);
		writer.WriteByteSafe((byte)messageType);
	}

	/// <summary>
	/// 读取并校验包头
	/// </summary>
	private bool ReadAndCheckHeader(FastBufferReader reader, MessageType expectedType) {
		reader.ReadValueSafe(out long receivedApplicationId);
		if (receivedApplicationId != m_UniqueApplicationId) { return false; }
		reader.ReadByteSafe(out byte messageType);
		if (messageType != (byte)expectedType) { return false; }
		return true;
	}
}