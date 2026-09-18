using UnityEngine;

public class SkillObject_ArrowPierce : SkillObject_Arrow
{
    private int amountToPierce;

    public override void SetupArrow(Skill_BowAttack bowManager, Vector2 direction)
    {
        base.SetupArrow(bowManager, direction);
        amountToPierce = bowManager.amountToPierce;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        bool groundHit = collision.gameObject.layer == LayerMask.NameToLayer("Ground");

        if (amountToPierce <= 0 && groundHit)
        {
            DamageEnemiesInRadius(transform, 0.3f);
            StopArrow(collision);
            return;
        }

        //avendo messo il raggio a 0.3 evito il rischio di colpire più volte due nemici vicini
        
        amountToPierce--;
        DamageEnemiesInRadius(transform, 0.3f);

    }
}
