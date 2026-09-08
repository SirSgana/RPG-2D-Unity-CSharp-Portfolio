using System;
using UnityEngine;

[Serializable]

public class AttackData
{
    public float physicalDamage;
    public float elementalDamage;
    public bool isCrit;
    public ElementType element;

    public ElementalEffectData effectData;

    public AttackData(Entity_Stats entity_Stats, DamageScaleData scaleData)
    {
        physicalDamage = entity_Stats.GetPhysicalDamage(out isCrit, scaleData.physical);
        elementalDamage = entity_Stats.GetElementalDamage(out element, scaleData.elemental);

        effectData = new ElementalEffectData(entity_Stats, scaleData);
    }
}
