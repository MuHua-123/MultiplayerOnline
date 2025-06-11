using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 碰撞角色 - 控制器
/// </summary>
public class CCharacterCollision : ControlCharacter {

	[Header("扩展功能")]
	public Animator animator;
	public CharacterController controller;
	public LayerMask ground;

	private MCharacterCollision mCharacter;

	public override MCharacter MCharacter => mCharacter;

	private void Awake() {
		mCharacter = new MCharacterCollision(animator, controller, ground);
	}
	private void Update() {
		mCharacter.Update();
	}
	public void AnimationExit() {
		mCharacter.AnimationExit();
	}
}
