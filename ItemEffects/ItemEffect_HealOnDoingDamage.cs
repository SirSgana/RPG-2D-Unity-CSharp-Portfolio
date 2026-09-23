using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Heal On Doing Damage", fileName = "Item Effect Data - Heal On Doing Physical Damage ")]

public class ItemEffect_HealOnDoingDamage : Item_EffectDataSO
{
    [SerializeField] private float percentHealOnAttack = 0.2f;

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.combat.OnDoingPhysicalDamage += HealOnDoingDamage;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.combat.OnDoingPhysicalDamage -= HealOnDoingDamage;
        player = null;
    }

    private void HealOnDoingDamage(float damage)
    {
        player.health.IncreaseHealth(damage * percentHealOnAttack);
    }
}
