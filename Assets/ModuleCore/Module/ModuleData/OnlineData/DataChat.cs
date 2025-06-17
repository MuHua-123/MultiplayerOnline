using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// 聊天数据
/// </summary>
public class DataChat : INetworkSerializable {
	/// <summary> 用户id </summary>
	public string id;
	/// <summary> 用户名称 </summary>
	public string name;
	/// <summary> 发送时间 </summary>
	public string time;
	/// <summary> 聊天内容 </summary>
	public string content;
	/// <summary> 是所有者 </summary>
	public bool isOwner;

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
		serializer.SerializeValue(ref id);
		serializer.SerializeValue(ref name);
		serializer.SerializeValue(ref time);
		serializer.SerializeValue(ref content);
	}
}
