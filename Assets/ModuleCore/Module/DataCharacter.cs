using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色 - 数据
/// </summary>
public class DataCharacter {

	/// <summary> 移动速度 </summary>
	public float moveSpeed = 2;
	/// <summary> 冲刺速度 </summary>
	public float sprintSpeed = 5.5f;
	/// <summary> 加速度 </summary>
	public float acceleration = 15;
	/// <summary> 跳跃高度 </summary>
	public float jumpHeight = 2;

	public DataCharacter(HCharacterCollision hCharacter) {
		moveSpeed = hCharacter.moveSpeed;
		sprintSpeed = hCharacter.sprintSpeed;
		acceleration = hCharacter.acceleration;
		jumpHeight = hCharacter.jumpHeight;
	}
	// public DataCharacter(HCharacterStandard hCharacter) {
	// 	moveSpeed = hCharacter.moveSpeed;
	// 	sprintSpeed = hCharacter.sprintSpeed;
	// 	acceleration = hCharacter.acceleration;
	// 	jumpHeight = hCharacter.jumpHeight;
	// }

}
