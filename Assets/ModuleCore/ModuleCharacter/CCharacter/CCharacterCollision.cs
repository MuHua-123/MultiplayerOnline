using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 碰撞 - 角色控制器
/// </summary>
public class CCharacterCollision : CCharacter {

	public DataCharacter dCharacter;
	public HCharacterCollision hCharacter;
	public MCharacterCollision mCharacter;

	public override MCharacter MCharacter => mCharacter;
	public override DataCharacter DCharacter => dCharacter;

	public override void Initial(Vector3 position, Vector3 eulerAngles) {
		hCharacter = GetComponent<HCharacterCollision>();
		mCharacter = new MCharacterCollision(hCharacter.animator, hCharacter.controller, hCharacter.ground);
		mCharacter.movement.Settings(position, eulerAngles);

		dCharacter = new DataCharacter(hCharacter);
	}
	private void Update() {
		mCharacter.Update();
	}
	public void AnimationExit() {
		mCharacter.AnimationExit();
	}
}
