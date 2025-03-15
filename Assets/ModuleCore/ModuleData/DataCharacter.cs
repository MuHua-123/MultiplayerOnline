using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using MuHua;

[Serializable]
public class DataCharacter : ModuleData<DataCharacter>, INetworkSerializable {

	//同步
	public ulong clientId;
	public Vector3 position;

	//执行
	public float delay;//延迟
	public Vector2 moveInput;//方向

	public DataCharacter() { }
	public DataCharacter(ulong clientId) => this.clientId = clientId;

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
		serializer.SerializeValue(ref clientId);
		serializer.SerializeValue(ref position);

		serializer.SerializeValue(ref delay);
		serializer.SerializeValue(ref moveInput);
	}

	public void Update() {
		VisualCharacter.I.UpdateVisual(this);
	}
	public void Update(DataCharacter character) {
		this.position = character.position;

		this.delay = character.delay;
		this.moveInput = character.moveInput;

		VisualCharacter.I.UpdateVisual(this);
	}
}
