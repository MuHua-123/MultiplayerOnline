using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStateMachine : MonoBehaviour {
    [Header("控制组件")]
    public CharacterMovement movement;

    [Header("漫游参数")]
    public float roamingSpeed = 2.0f;
    public float roamingWaitTime = 5.0f; // 等待时间

    [Header("追击参数")]
    public Transform player;
    public float chasingSpeed = 4.0f;

    [Header("攻击参数")]
    public float attackRange = 1.5f;
    public float detectionRange = 10.0f;

    private CharacterState currentState;
    private Dictionary<CharacterStateType, CharacterState> states = new Dictionary<CharacterStateType, CharacterState>();

    private void Start() {
        InitializeStates();
        ChangeState(CharacterStateType.Roaming);
    }

    private void Update() {
        currentState?.Update();
    }

    protected abstract void InitializeStates();

    protected void RegisterState(CharacterStateType stateType, CharacterState state) {
        if (!states.ContainsKey(stateType)) {
            states.Add(stateType, state);
        }
    }

    public void ChangeState(CharacterStateType stateType) {
        if (states.ContainsKey(stateType)) {
            currentState?.Exit();
            currentState = states[stateType];
            currentState.Enter();
        }
        else {
            Debug.LogWarning($"State {stateType} is not registered.");
        }
    }
}
