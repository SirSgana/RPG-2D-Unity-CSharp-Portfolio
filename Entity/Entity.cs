using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public event Action OnFlipped;

    protected StateMachine stateMachine;

    public Rigidbody2D rb { get; private set; }
    public Entity_Stats stats { get; private set; }
    public Animator anim { get; private set; }

    private bool facingRight = true;
    public int facingDir { get; private set; } = 1;

    [Header("Collision detection")]
    public LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    //Condition variables
    private bool isKnocked;
    private Coroutine knockbackCoroutine;
    private Coroutine slowDownCo;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>(); //Deve rimanere per primo altrimenti non verrà trovato quando vogliamo le variabili
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Entity_Stats>();

        stateMachine = new StateMachine();
    }

    protected virtual void Start() { }

    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState(); //Riferimento alla funzione in StateMachine.cs
    }

    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }

    public virtual void EntityDead() { } //Devo tenerla se no non va

    public virtual void SlowDownEntity(float duration, float slowMultiplier)
    {
        if (slowDownCo != null)
            StopCoroutine(slowDownCo);

        slowDownCo = StartCoroutine(SlowDownEntityCo(duration, slowMultiplier));

    }

    protected virtual IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        yield return null; //qui non serve scrivere niente perchè verrà eseguito un Override in altri script
    }

    public void ReciveKnockback(Vector2 knockback, float duration)
    {
        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);

        knockbackCoroutine = StartCoroutine(KnockbackCo(knockback, duration));
    }

    private IEnumerator KnockbackCo(Vector2 knockback, float duration)
    {
        isKnocked = true;
        rb.linearVelocity = knockback;

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero; //Serve per azzerare la forza anche se non viene premuto nulla
        isKnocked = false;
    }
    public void SetVelocity(float xVelocity, float yVelocity) //Metodo creato per chiamare la velocità del player senza dover
    {                                                         //richiamare sempre la funzione che sta sotto
        if (isKnocked)
            return;
        
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && facingRight == false)
            Flip();
        else if (xVelocity < 0 && facingRight == true)
            Flip();

        OnFlipped?.Invoke();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir = facingDir * -1;
    }

    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        if (secondaryWallCheck != null)
        {
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround)
                        && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
        }
        else
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance, 0));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0, 0));

        if (secondaryWallCheck != null)
            Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0, 0));
    }
}
