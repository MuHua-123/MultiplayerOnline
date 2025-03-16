using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : CharacterStateMachine {
    protected override void InitializeStates() {
        RegisterState(CharacterStateType.Idle, new IdleState(this));
        RegisterState(CharacterStateType.Roaming, new RoamingState(this));
        RegisterState(CharacterStateType.Chasing, new ChasingState(this));
        RegisterState(CharacterStateType.Attack, new AttackState(this));
    }
}
