using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    public float moveSpeed = 5.0f; // 最大移动速度
    public float acceleration = 20.0f; // 加速度
    private float currentSpeed = 0.0f; // 当前速度

    private Vector2 moveInput; // 移动输入
    private Animator animator; // 动画控制器
    private bool isEnabled = true; // 移动是否启用
    private bool isLockedOn = false; // 是否锁定目标
    private Vector3 lockOnPosition; // 锁定位置
    private Vector3 knockbackVelocity; // 击退速度
    private bool isKnockbackActive = false; // 是否处于击退状态

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Enable() {
        isEnabled = true;
    }

    public void Disable() {
        isEnabled = false;
        currentSpeed = 0.0f; // 禁用时重置当前速度
        animator.SetFloat("MoveSpeed", 0.0f); // 更新动画参数
        animator.SetFloat("MoveX", 0.0f);
        animator.SetFloat("MoveZ", 0.0f);
    }

    public void SetMoveInput(Vector2 input) {
        moveInput = input;
    }

    public void LockOn(bool lockOn, Vector3 position) {
        isLockedOn = lockOn;
        lockOnPosition = position;
    }

    public void ApplyKnockback(Vector3 knockback) {
        knockbackVelocity = knockback;
        isKnockbackActive = true;
    }

    private void Update() {
        if (!isEnabled) return;

        // 处理击退效果
        if (knockbackVelocity != Vector3.zero) {
            transform.position += knockbackVelocity * Time.deltaTime;
            knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, Time.deltaTime * 5);

            if (knockbackVelocity.magnitude < 0.1f) {
                knockbackVelocity = Vector3.zero;
                isKnockbackActive = false;
            }
        }

        // 根据输入移动玩家
        if (!isKnockbackActive) { MoveCharacter(); }

        // 如果有移动输入或锁定目标，则更新玩家的朝向
        if (moveInput != Vector2.zero) {
            if (isLockedOn) {
                RotateCharacterLocked();
            }
            else {
                RotateCharacter();
            }
        }

        animator.SetBool("Move", moveInput != Vector2.zero);
    }

    private void MoveCharacter() {
        // 计算相对于世界坐标系的移动方向
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        // 平滑加速和减速
        currentSpeed = moveInput != Vector2.zero
            ? Mathf.MoveTowards(currentSpeed, moveSpeed, acceleration * Time.deltaTime)
            : Mathf.MoveTowards(currentSpeed, 0, acceleration * Time.deltaTime);

        // 移动玩家
        transform.Translate(moveDirection * currentSpeed * Time.deltaTime, Space.World);

        // 计算转向向量
        Vector3 localMoveDirection = transform.InverseTransformDirection(moveDirection * currentSpeed);
        localMoveDirection = localMoveDirection.normalized;
        // 对localMoveDirection的x和z进行分类处理
        float moveX = CategorizeDirection(localMoveDirection.x);
        float moveZ = CategorizeDirection(localMoveDirection.z);

        // 更新动画参数
        animator.SetFloat("MoveSpeed", currentSpeed);
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveZ", moveZ);
    }

    private void RotateCharacter() {
        // 计算相对于世界坐标系的移动方向
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        // 如果有移动输入，则更新玩家的朝向
        if (moveDirection != Vector3.zero) {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, moveSpeed * Time.deltaTime * 100);
        }
    }

    private void RotateCharacterLocked() {
        // 计算锁定位置的方向
        Vector3 lockOnDirection = (lockOnPosition - transform.position).normalized;

        // 如果有锁定目标，则更新玩家的朝向
        if (lockOnDirection != Vector3.zero) {
            Quaternion toRotation = Quaternion.LookRotation(lockOnDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, moveSpeed * Time.deltaTime * 100);
        }
    }

    private float CategorizeDirection(float value) {
        if (value < -0.4f) { return -1f; }
        else if (value > 0.4f) { return 1f; }
        else { return 0f; }
    }
}
