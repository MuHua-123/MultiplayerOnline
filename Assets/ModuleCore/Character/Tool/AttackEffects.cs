using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackEffects : MonoBehaviour {

    [Header("攻击效果")]
    public Transform attackEffect1;
    public Transform attackEffect2;
    public Transform attackEffect3;
    public Transform attackEffect4;
    public Transform attackEffect5;
    public Transform attackEffect6;
    public Transform attackEffect7;

    public void EnableEffects(int index) {
        if (index == 1) { EnableEffects(attackEffect1); }
        if (index == 2) { EnableEffects(attackEffect2); }
        if (index == 3) { EnableEffects(attackEffect3); }
        if (index == 4) { EnableEffects(attackEffect4); }
        if (index == 5) { EnableEffects(attackEffect5); }
        if (index == 6) { EnableEffects(attackEffect6); }
        if (index == 7) { EnableEffects(attackEffect7); }
    }

    public void EnableEffects(Transform obj) {
        Transform effect = Instantiate(obj);
        effect.gameObject.SetActive(true);
        effect.position = obj.position;
        effect.eulerAngles = obj.eulerAngles;
    }

    public void Hit(Transform obj, Vector3 closestPoint) {

    }
}
