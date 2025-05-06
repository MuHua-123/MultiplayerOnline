using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using MuHua;

[Serializable]
public struct DataCharacter : INetworkSerializable {

	// 同步
	public Vector3 position;
	public Vector3 eulerAngles;
	// 输入
	public Vector2 moveInput;

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
		// 同步序列化
		serializer.SerializeValue(ref position);
		serializer.SerializeValue(ref eulerAngles);
		// 输入序列化
		serializer.SerializeValue(ref moveInput);
	}

	public void Update(Vector2 moveInput, KinesisController controller) {
		this.moveInput = moveInput;
		position = controller.transform.position;
		eulerAngles = controller.transform.eulerAngles;
	}
}
