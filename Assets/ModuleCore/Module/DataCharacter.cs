using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色 - 数据
/// </summary>
public class DataCharacter {

	/// <summary> 移动速度 </summary>
	public float moveSpeed = 1;
	/// <summary> 加速度 </summary>
	public float acceleration = 15;
	/// <summary> 跳跃高度 </summary>
	public float jumpHeight = 2;

	public DataCharacter(HCharacterCollision hCharacter) {
		moveSpeed = hCharacter.moveSpeed;
		acceleration = hCharacter.acceleration;
		jumpHeight = hCharacter.jumpHeight;
	}

}
