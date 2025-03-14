using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using MuHua;

[Serializable]
public class DataCharacter : ModuleData<DataCharacter>, INetworkSerializable {
	public Vector3 position;

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
		serializer.SerializeValue(ref position);
	}
}
