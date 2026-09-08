using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private const int FirstComboIndex = 1; //Questa è una costante che non può essere modificata ci serve per evitare "magic Number"
                                           //per ricominciare la combo (che inizia con il numero 1
                                           //Le costanti iniziano con la lettera maiuscola convenzione di Microsoft per distinguerle

    private float attackVelocityTimer;
    private float lastTimeAttack;

    private int comboIndex = 1; //Variabile per la combo parte da 1 perchè ci viene comodo per comprendere meglio
    private int comboLimit = 3;
    private int attackDir;

    private bool comboAttackQueued;

    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if (comboLimit != player.attackVelocity.Length)
        {
            Debug.LogWarning(" Il sistema degli array per la combo è stato sistemato per l'attackVelocity! ");
            comboLimit = player.attackVelocity.Length;
        }
    }

    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        ResetComboIndexIfNeeded();
        SyncAttackSpeed();

        attackDir = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDir;
        //Questa riga sopra chiede è attackDir = ecc.. ? se è si usa la prima : (altrimenti) : usa la seconda
        //è la prima volta che uso questo metodo quindi per maggior comprensione lascio qui sotto l'equivalente in IF
        //if (player.moveInput.x != 0)
        //    attackDir = ((int)player.moveInput.x); //è stato aggiunto un int perchè il valore di attackDir è int mentre il player no quindi
        //                                           //per convertirlo ho dovuto inserire int
        //else
        //    attackDir = player.facingDir;

        anim.SetInteger("basicAttackIndex", comboIndex);
        ApplyAttackVelocity();
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (input.Player.Attack.WasPressedThisFrame())
            QueueNextAttack();

        if (triggerCalled)
            HandleStateExit();
    }

    public override void Exit()
    {
        base.Exit();

        comboIndex++;
        lastTimeAttack = Time.time;
    }

    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
            stateMachine.ChangeState(player.idleState);
    }
    
    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
            comboAttackQueued = true;
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        if (attackVelocityTimer < 0)
            player.SetVelocity(0, rb.linearVelocity.y);
    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1]; //Avendo iniziato le combo con 1 il -1 mi permette di prendere l'elemento 0 
        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y);
    }

    private void ResetComboIndexIfNeeded()
    {
        if (comboIndex > comboLimit || Time.time > lastTimeAttack + player.comboResetTime)
            comboIndex = FirstComboIndex;
    }
}
