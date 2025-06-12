using System.Net;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// 响应 - 数据
/// </summary>
public struct DataDiscoveryResponse : INetworkSerializable {

	public IPAddress address;

	public ushort Port;
	public string ServerName;
	public string gameVersion;

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
		serializer.SerializeValue(ref Port);
		serializer.SerializeValue(ref ServerName);
		serializer.SerializeValue(ref gameVersion);
	}
}
