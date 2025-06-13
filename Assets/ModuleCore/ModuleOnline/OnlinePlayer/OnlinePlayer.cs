using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using MuHua;

/// <summary>
/// 玩家 - 联机
/// </summary>
public class OnlinePlayer : NetworkBehaviour {
	[Header("联机组件")]
	public OnlineCharacter onlineCharacter;
	public OnlineCharacterControl onlineCharacterControl;
	[Header("运行模块")]
	public ControlCharacter control;

}