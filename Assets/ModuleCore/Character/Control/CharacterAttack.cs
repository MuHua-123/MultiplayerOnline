using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour {
    public float attackRange = 2.0f; // 攻击范围
    public float attackCooldown = 1.0f; // 攻击冷却时间
    public float knockbackForce = 5.0f; // 击退力
    public bool comboTriggered = false; // 是否触发连击
    public Vector3 targetPosition; // 攻击目标位置

    private Animator animator; // 动画控制器
    private CharacterMovement characterMovement; // 玩家移动组件
    private bool isLocked = false; // 是否锁定

    private void Awake() {
        animator = GetComponent<Animator>();
        characterMovement = GetComponent<CharacterMovement>();
    }

    public void Attack() {
        // 处于锁定状态，不能攻击
        if (!isLocked) { ComboAttack(); }
    }

    private void ComboAttack() {
        // 计算攻击动画速度
        float attackSpeed = 1.0f / attackCooldown;

        // 播放攻击动画
        animator.SetFloat("AttackSpeed", attackSpeed);

        // 执行攻击
        LockCharacter(); // 锁定角色

        // 计算目标方向
        Vector3 direction = (targetPosition - transform.position).normalized;

        // 使角色面向攻击方向
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    public void Hit(Transform target) {
        // 计算击退方向
        Vector3 knockbackDirection = (target.position - transform.position).normalized;

        // 应用击退力
        CharacterMovement targetMovement = target.GetComponent<CharacterMovement>();
        if (targetMovement != null) {
            Vector3 knockback = knockbackDirection * knockbackForce;
            targetMovement.ApplyKnockback(knockback);
        }
    }

    private void LockCharacter() {
        isLocked = true;
        animator.SetBool("Attack", isLocked);
        characterMovement.Disable(); // 禁用角色移动

    }

    public void UnlockCharacter() {
        if (comboTriggered) { ComboAttack(); return; }
        isLocked = false;
        animator.SetBool("Attack", false);
        characterMovement.Enable(); // 启用角色移动
    }

}
