using UnityEngine;

public class AttackState : CharacterState {
    public AttackState(CharacterStateMachine character) : base(character) { }

    public override void Enter() {
        //Debug.Log("Attacking the player!");
    }

    public override void Update() {
        if (Vector3.Distance(character.transform.position, character.player.position) > character.attackRange) {
            character.ChangeState(CharacterStateType.Chasing);
        }
    }

    public override void Exit() { }
}

