using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffects : MonoBehaviour {
    public CharacterAttack characterAttack;
    public AttackEffects attackEffects;

    public void EnableEffects(int index) => attackEffects.EnableEffects(index);

    public void AttackUnlock() => characterAttack.UnlockCharacter();
}
