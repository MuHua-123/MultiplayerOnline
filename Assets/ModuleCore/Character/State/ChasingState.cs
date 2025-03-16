using UnityEngine;

public class ChasingState : CharacterState {
    private CharacterMovement characterMovement;

    public ChasingState(CharacterStateMachine character) : base(character) {
        characterMovement = character.movement;
    }

    public override void Enter() {
        // 设置追击速度
        characterMovement.moveSpeed = character.chasingSpeed;
    }

    public override void Update() {
        Vector3 direction = (character.player.position - character.transform.position).normalized;
        Vector2 moveInput = new Vector2(direction.x, direction.z);

        characterMovement.SetMoveInput(moveInput);

        if (Vector3.Distance(character.transform.position, character.player.position) < character.attackRange) {
            character.ChangeState(CharacterStateType.Attack);
        }

        if (Vector3.Distance(character.transform.position, character.player.position) > character.detectionRange) {
            character.ChangeState(CharacterStateType.Roaming);
        }
    }

    public override void Exit() {
        characterMovement.SetMoveInput(Vector2.zero); // 停止移动
    }
}

