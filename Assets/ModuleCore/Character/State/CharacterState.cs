using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState {
    protected CharacterStateMachine character;

    public CharacterState(CharacterStateMachine character) {
        this.character = character;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
public enum CharacterStateType {
    Idle,
    Roaming,
    Chasing,
    Attack
}