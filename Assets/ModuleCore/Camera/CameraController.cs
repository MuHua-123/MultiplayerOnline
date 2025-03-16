using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour {
    public Transform player; // 玩家对象
    public Vector3 offset; // 相机与玩家的偏移量
    [Range(0, 0.5f)] public float smoothSpeed = 0.125f; // 平滑跟随速度

    private Vector3 eulerAngles;
    private bool isRotating = false;

    private void Start() {
        eulerAngles = transform.eulerAngles;
    }

    private void LateUpdate() {
        // 计算目标位置
        Vector3 desiredPosition = player.position + offset;
        // 平滑过渡到目标位置
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public void OnEnableRotating(InputValue inputValue) {
        isRotating = inputValue.isPressed;
    }

    public void OnRotateCamera(InputValue inputValue) {
        if (!isRotating) { return; }
        Vector2 delta = inputValue.Get<Vector2>();
        // 计算旋转角度
        eulerAngles += new Vector3(-delta.y * 0.2f, delta.x * 0.5f, 0);
        transform.eulerAngles = eulerAngles;
    }
}

