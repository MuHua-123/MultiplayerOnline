using Unity.Netcode;
using UnityEngine;

public struct DataDiscoveryBroadcast : INetworkSerializable {
	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter { }
}
