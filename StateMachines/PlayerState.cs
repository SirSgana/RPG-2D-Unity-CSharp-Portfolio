public abstract class PlayerState : EntityState
{
    protected Player player;
    protected Player_InputSystem input;
    protected Player_SkillManager skillManager;

    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName) //Costruttore che accetta un riferimento allo StateMachine
    {
        this.player = player;

        anim = player.anim;           //Questo mi serve per evitare di scrivere sempre player.anim(SetBool) ad esempio in ogni riga per ogni cosa
        rb = player.rb;               //Questo mi serve per evitare di chiamare sempre player.rb.linearVelocity ad esempio
        input = player.input;         //Questo prende il Player_InputSystem
        stats = player.stats;         //Questo l'ho aggiunto quando ho inserito il regenera vita
        skillManager = player.skillManager; //Questo consente l'accesso al player per lo script Skill_Manager
    }

    public override void Update()
    {
        base.Update();

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            skillManager.dash.SetSkillOnCooldown();
            stateMachine.ChangeState(player.dashState);
        }

        if (input.Player.UltimateSpell.WasPressedThisFrame() && skillManager.domainExpansion.CanUseSkill())
        {
            if (skillManager.domainExpansion.InstantDomain())
                skillManager.domainExpansion.CreateDomain();
            else
                stateMachine.ChangeState(player.domainExpansionState);

            skillManager.domainExpansion.SetSkillOnCooldown();
        }
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private bool CanDash()
    {
        if (skillManager.dash.CanUseSkill() == false)
            return false;
       
        if (player.wallDetected)
            return false;

        if (stateMachine.currentState == player.dashState)
            return false;

        return true;
    }
}
