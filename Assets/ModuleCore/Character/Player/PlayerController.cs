using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    public CameraController cameraController; // 相机对象
    public CharacterMovement playerMovement; // 玩家移动组件
    public CharacterAttack characterAttack; // 玩家攻击组件
    public Vector2 screenPosition; // 屏幕坐标

    private Vector2 moveInput;

    public void OnMove(InputValue inputValue) {
        // 获取移动输入
        moveInput = inputValue.Get<Vector2>();
    }

    public void OnAttack(InputValue inputValue) {
        // 调用 CharacterAttack 的 Attack 方法
        characterAttack.Attack();
    }

    public void OnCombo(InputValue inputValue) {
        characterAttack.comboTriggered = inputValue.isPressed;
    }

    public void OnTargeted(InputValue inputValue) {
        // 屏幕坐标
        screenPosition = inputValue.Get<Vector2>();
    }

    private void Update() {
        // 获取相机的前向和右向
        Vector3 cameraForward = cameraController.transform.forward;
        Vector3 cameraRight = cameraController.transform.right;

        // 忽略相机的y轴
        cameraForward.y = 0;
        cameraRight.y = 0;

        // 归一化向量
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 worldPosition = playerMovement.transform.position + cameraForward.normalized;
        playerMovement.LockOn(true, worldPosition);
        characterAttack.targetPosition = worldPosition;

        // 计算相对于相机的移动方向
        Vector3 moveDirection = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

        // 调用 SetMoveInput 方法并传递相对于玩家的移动方向
        playerMovement.SetMoveInput(new Vector2(moveDirection.x, moveDirection.z));
    }
}

