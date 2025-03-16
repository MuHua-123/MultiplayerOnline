using UnityEngine;

public class RoamingState : CharacterState {
    private Vector3 roamingTarget;
    private CharacterMovement characterMovement;
    private float waitTimer = 0.0f; // 计时器

    public RoamingState(CharacterStateMachine character) : base(character) {
        characterMovement = character.movement;
    }

    public override void Enter() {
        SetNewRoamingTarget();
        characterMovement.moveSpeed = character.roamingSpeed; // 应用漫游速度
    }

    public override void Update() {
        // 检测玩家是否在追击范围内
        if (Vector3.Distance(character.transform.position, character.player.position) < character.detectionRange) {
            character.ChangeState(CharacterStateType.Chasing); return;
        }

        // 如果到达漫游目标，设置新的漫游目标
        if (Vector3.Distance(character.transform.position, roamingTarget) < 0.5f) {
            characterMovement.SetMoveInput(Vector2.zero); // 停止移动
            if (waitTimer > 0) {
                waitTimer -= Time.deltaTime;
                return;
            }
            waitTimer = character.roamingWaitTime; // 开始等待
            SetNewRoamingTarget();
        }

        Vector3 direction = (roamingTarget - character.transform.position).normalized;
        Vector2 moveInput = new Vector2(direction.x, direction.z);

        characterMovement.SetMoveInput(moveInput);
    }

    public override void Exit() {
        characterMovement.SetMoveInput(Vector2.zero); // 停止移动
    }

    private void SetNewRoamingTarget() {
        // 设置一个新的漫游目标
        roamingTarget = character.transform.position + new Vector3(
            Random.Range(-10.0f, 10.0f),
            0,
            Random.Range(-10.0f, 10.0f)
        );
    }
}
