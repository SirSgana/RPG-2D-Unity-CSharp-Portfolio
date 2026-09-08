using UnityEngine;

public class Player_GroundedState : PlayerState
{
    //Questo sarà un "SuperState" cioè un collegamento per gli stati Idle e Move dove implementeremo anche Jump Attack ecc...
    //Ora che abbiamo creato un SuperState abbiamo ereditato da esso l'entityState e su Idle e Move abbiamo ereditato questo
    public Player_GroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0 && player.groundDetected == false)
            stateMachine.ChangeState(player.fallState);

        if (input.Player.Jump.WasPressedThisFrame())
            stateMachine.ChangeState(player.jumpState);

        if (input.Player.Attack.WasPressedThisFrame())
            stateMachine.ChangeState(player.basicAttackState);

        if (input.Player.CounterAttack.WasPressedThisFrame())
            stateMachine.ChangeState(player.counterAttackState);

        if (input.Player.BowAttack.WasPressedThisFrame() && skillManager.bow.CanUseSkill())
            stateMachine.ChangeState(player.bowState);
    }
}
