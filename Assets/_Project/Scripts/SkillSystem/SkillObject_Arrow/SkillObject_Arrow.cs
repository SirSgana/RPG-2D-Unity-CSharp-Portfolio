using UnityEngine;

public class SkillObject_Arrow : SkillObject_Base
{
    protected Skill_BowAttack bowAttack;
    protected Transform playerTransform;
    protected bool shouldComeback;
    protected float comebackSpeed = 20f;
    protected float maxAllowedDistance = 25;

    protected virtual void Update()
    {
        transform.right = rb.linearVelocity;
        HandleComeback();
    }

    public virtual void SetupArrow(Skill_BowAttack bowManager, Vector2 direction)
    {
        rb.linearVelocity = direction;

        this.bowAttack = bowManager;

        playerTransform = bowManager.transform.root;
        playerStats = bowManager.player.stats;
        damageScaleData = bowManager.damageScaleData;
    }

    public void GetArrowBackToPlayer() => shouldComeback = true;

    protected void HandleComeback()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > maxAllowedDistance)
            GetArrowBackToPlayer();

        if (shouldComeback == false)
            return;

        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, comebackSpeed * Time.deltaTime);

        if (distance < 0.5f)
            Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        StopArrow(collision);
        DamageEnemiesInRadius(transform, 1);
    }

    protected void StopArrow(Collider2D collision)
    {
        rb.simulated = false;
        transform.parent = collision.transform;
    }
}
