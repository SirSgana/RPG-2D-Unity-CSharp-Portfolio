using UnityEngine;

public class Player_BowState : PlayerState
{
    private Camera mainCamera;

    public Player_BowState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    override public void Enter()
    {
        base.Enter();

        skillManager.bow.EnableDots(true);

        if (mainCamera != Camera.main);
            mainCamera = Camera.main;
    }

    public override void Update()
    {
        base.Update();

        Vector2 dirToMouse = DirectionToMouse();

        player.SetVelocity(0, rb.linearVelocity.y);
        player.HandleFlip(dirToMouse.x);
        skillManager.bow.PredictTrajectory(dirToMouse);

        if (input.Player.Attack.WasPressedThisFrame())
        {
            anim.SetBool("bowAttackPerformed", true);

            skillManager.bow.EnableDots(false);
            skillManager.bow.ConfirmTrajectory(dirToMouse);

        }

        if (input.Player.BowAttack.WasPressedThisFrame() || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("bowAttackPerformed", false);
        skillManager.bow.EnableDots(false);
    }

    private Vector2 DirectionToMouse()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 worldMousePosition = mainCamera.ScreenToWorldPoint(player.mousePosition);

        Vector2 direction = worldMousePosition - playerPosition;

        return direction.normalized;
    }
}
