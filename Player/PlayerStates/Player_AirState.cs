using UnityEngine;

public class Player_AirState : PlayerState
{
    //Queto sarà un SuperState per tutto ciò che sarà un azione aerea
    public Player_AirState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x != 0)
            player.SetVelocity(player.moveInput.x * (player.moveSpeed * player.inAirMultiplier), rb.linearVelocity.y);

        if (input.Player.Attack.WasPressedThisFrame())
            stateMachine.ChangeState(player.airAttackState);
    }
}
