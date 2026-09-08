using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine; //Riferimento allo script StateMachine
    protected Animator anim; //Capisci il perchè alla riga 22
    protected Rigidbody2D rb;
    protected Entity_Stats stats;

    protected string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;

    public EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() //Metodo chiamato quando si entra nello stato
    {
        anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update() //Metodo chiamato ogni frame per aggiornare la logica dello stato 
                                 //Data la mancanza di MonoBehaviour, non possiamo usare Update. Esso verrà chiamato dal Player Script
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();
    }

    public virtual void Exit() //Metodo chiamato quando si esce dallo stato
    {
        anim.SetBool(animBoolName, false);
    }

    public void AnimationTrigger()
    {
        triggerCalled = true;
    }

    public virtual void UpdateAnimationParameters()
    {

    }

    public void SyncAttackSpeed()
    {
        float attackSpeed = stats.offense.attackSpeed.GetValue();
        anim.SetFloat("attackSpeedMultiplier", attackSpeed);
    }
}
