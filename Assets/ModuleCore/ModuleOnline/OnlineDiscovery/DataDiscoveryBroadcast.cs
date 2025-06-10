using Unity.Netcode;
using UnityEngine;

/// <summary>
/// 广播 - 数据
/// </summary>
public struct DataDiscoveryBroadcast : INetworkSerializable {
	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter { }
}
