using UnityEngine;

public class Player_MoveState : Player_GroundedState
{
    public Player_MoveState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x == 0 || player.wallDetected)
            stateMachine.ChangeState(player.idleState);

        //Qui sotto dovrebbe essere "player.rb.linearVelocity.y" ma visto che su EntityState abbiamo la funzione non serve player.
        player.SetVelocity(player.moveInput.x * player.moveSpeed, rb.linearVelocity.y);
    }
}
