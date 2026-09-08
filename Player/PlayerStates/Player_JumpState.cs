using UnityEngine;

public class Player_JumpState : Player_AirState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();
        //questo && serve perchè quando ho implementato l'attacco in salto non si attivava quando ho implementato la velocità di discesa in airAttackState
        if (rb.linearVelocity.y < 0 && stateMachine.currentState != player.airAttackState)
            stateMachine.ChangeState(player.fallState);
    }
}
