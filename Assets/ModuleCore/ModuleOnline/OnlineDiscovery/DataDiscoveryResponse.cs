using System.Net;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// 响应 - 数据
/// </summary>
public struct DataDiscoveryResponse : INetworkSerializable {

	public IPAddress address;

	/// <summary> 服务器端口 </summary>
	public ushort port;
	/// <summary> 服务器名称 </summary>
	public string serverName;
	/// <summary> 服务器版本 </summary>
	public string serverVersion;

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
		serializer.SerializeValue(ref port);
		serializer.SerializeValue(ref serverName);
		serializer.SerializeValue(ref serverVersion);
	}
}
