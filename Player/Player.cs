using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    private UI ui;
    public static event Action OnPlayerDeath;
    public Player_InputSystem input { get; private set; }
    public Player_SkillManager skillManager { get; private set; }
    public Player_VFX vfx { get; private set; }
    public Entity_Health health { get; private set; }
    public Entity_StatusHandler statusHandler { get; private set; }

    #region State Variables

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_AirAttackState airAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }
    public Player_BowState bowState { get; private set; }
    public Player_DomainExpansionState domainExpansionState { get; private set; }

    #endregion

    [Header("AirAttack details")]
    public Vector2 airAttackVelocity;

    [Header("Attack details")]
    public Vector2[] attackVelocity;
    public float attackVelocityDuration = 0.1f;
    public float comboResetTime = 1;
    private Coroutine queuedAttackCo; //Co sta per Coroutine

    [Header("Ultimate Ability details")]
    public float riseSpeed = 25f;
    public float riseMaxDistance = 3f;

    [Header("Movement details")]
    public float moveSpeed;
    public float jumpForce = 5;
    [Range(0, 1)]
    public float inAirMultiplier = 0.7f;
    [Range(0, 1)]
    public float wallSlideSlowMultiplier = 0.7f;
    [Space]
    public float dashDuration = 0.25f;
    public float dashSpeed = 20;
    public Vector2 wallJumpForce;
    public Vector2 moveInput { get; private set; }  //Agevola la comprensione dello script
    public Vector2 mousePosition { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        ui = FindAnyObjectByType<UI>();
        vfx = GetComponent<Player_VFX>();
        health = GetComponent<Entity_Health>();
        skillManager = GetComponent<Player_SkillManager>();
        statusHandler = GetComponent<Entity_StatusHandler>();
       
        input = new Player_InputSystem();

        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        airAttackState = new Player_AirAttackState(this, stateMachine, "airAttack");
        deadState = new Player_DeadState(this, stateMachine, "dead");
        counterAttackState = new Player_CounterAttackState(this, stateMachine, "counterAttack");
        bowState = new Player_BowState(this, stateMachine, "bowAttack");
        domainExpansionState = new Player_DomainExpansionState(this, stateMachine, "jumpFall");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState); //Inizializza lo stato nella StateMachine.cs
    }

    public void TeleportPlayer(Vector3 position) => transform.position = position;

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalJumpForce = jumpForce;
        float originalAnimSpeed = anim.speed;
        Vector2 originalWallJump = wallJumpForce;
        Vector2 originalJumpAttack = airAttackVelocity;
        Vector2[] originalAttackVelocity = new Vector2[attackVelocity.Length];
        Array.Copy(attackVelocity, originalAttackVelocity, attackVelocity.Length);

        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed = moveSpeed * speedMultiplier;
        jumpForce = jumpForce * speedMultiplier;
        anim.speed = anim.speed * speedMultiplier;
        wallJumpForce = wallJumpForce * speedMultiplier;
        airAttackVelocity = airAttackVelocity * speedMultiplier;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = attackVelocity[i] * speedMultiplier;
        }

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        jumpForce = originalJumpForce;
        anim.speed = originalAnimSpeed;
        wallJumpForce = originalWallJump;
        airAttackVelocity = originalJumpAttack;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = originalAttackVelocity[i];
        }
    }

    public override void EntityDead()
    {
        base.EntityDead();

        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deadState);
    }

    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCo != null)
            StopCoroutine(queuedAttackCo);

        queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }
    private void OnEnable()
    {
        input.Enable(); //Abilita l'inputSystem dall'awake e ci permette di utilizzarlo (altrimenti non si muoverebbe nemmeno)

        input.Player.Mouse.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();

        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;

        input.Player.ToggleSkillTreeUI.performed += ctx => ui.ToggleSkillTreeUI();

        input.Player.Spell.performed += ctx => skillManager.shard.TryUseSkill();

        input.Player.Spell_TimeEcho.performed += ctx => skillManager.timeEcho.TryUseSkill();
    }

    private void OnDisable()
    {
        input.Disable(); //Disabilita ciò che è all'interno dell'Enable del player in questo caso l'input
                         //Serve ad esempio impedisce il movimento dal mercante
    }
}
