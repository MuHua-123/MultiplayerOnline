using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour {
    public AttackEffects attackEffects;
    [Header("命中效果")]
    public ParticleSystem hitEffect; // 命中效果粒子系统

    private void OnTriggerEnter(Collider other) {
        // 调试输出碰撞到的物体名称
        Debug.Log("检测到碰撞物体: " + other.gameObject.name);

        // 获取碰撞点
        if (other is MeshCollider || other is BoxCollider || other is SphereCollider || other is CapsuleCollider) {
            // 获取碰撞体的碰撞点
            Vector3 closestPoint = other.ClosestPoint(transform.position);
            Debug.Log("碰撞点: " + closestPoint);

            // 在命中位置创建命中效果
            ParticleSystem effect = Instantiate(hitEffect, closestPoint, Quaternion.identity);
            effect.Play();

            // 触发攻击效果
            attackEffects.Hit(other.transform, closestPoint);
        }
    }
}
