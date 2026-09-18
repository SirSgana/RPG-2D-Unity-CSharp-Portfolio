using UnityEngine;

public class SkillObject_ArrowDrill : SkillObject_Arrow
{
    private int maxDistance;
    private float attacksPerSecond;
    private float attackTimer;

    public override void SetupArrow(Skill_BowAttack bowManager, Vector2 direction)
    {
        base.SetupArrow(bowManager, direction);

        anim?.SetTrigger("Drill");

        maxDistance = bowManager.maxDistance;
        attacksPerSecond = bowManager.attacksPerSecond;

        Invoke(nameof(GetArrowBackToPlayer), bowManager.maxDrillDuration);
    }

    protected override void Update()
    {
        base.Update();

        HandleAttack();
        HandleStopping();
    }
    private void HandleStopping()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > maxDistance && rb.simulated == true)
            rb.simulated = false;
    }
    
    private void HandleAttack()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer < 0)
        {
            DamageEnemiesInRadius(transform, 1);
            attackTimer = 1 / attacksPerSecond;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        rb.simulated = false;
    }


}
