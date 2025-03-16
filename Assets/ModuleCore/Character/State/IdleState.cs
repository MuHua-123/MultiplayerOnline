using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : CharacterState {
    public IdleState(CharacterStateMachine character) : base(character) {
    }

    public override void Enter() {
        // 进入空闲状态的逻辑
        Debug.Log("进入空闲状态");
    }

    public override void Update() {
        // 空闲状态的更新逻辑
        // 例如，检测是否需要转换到其他状态
    }

    public override void Exit() {
        // 退出空闲状态的逻辑
        Debug.Log("退出空闲状态");
    }
}
