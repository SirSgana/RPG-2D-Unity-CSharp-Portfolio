using UnityEngine;

public class Player_AirAttackState : PlayerState
{

    private bool touchedGround;

    public Player_AirAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        touchedGround = false;

        player.SetVelocity(player.airAttackVelocity.x * player.facingDir, player.airAttackVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (player.groundDetected && touchedGround == false)
        {
            touchedGround = true;
            anim.SetTrigger("airAttackTrigger");
            player.SetVelocity(0, rb.linearVelocity.y);
        }

        if (triggerCalled && player.groundDetected)
            stateMachine.ChangeState(player.idleState);
    }
}
