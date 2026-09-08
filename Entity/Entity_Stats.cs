using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat_SetupSO defaultStatSetup;

    public Stat_ResourceGroup resource;
    public Stat_OffenseGroup offense;
    public Stat_DifenseGroup defense;
    public Stat_MajorGroup major;

    public AttackData GetAttackData(DamageScaleData scaleData)
    {
        return new AttackData(this, scaleData);
    }
    

    public float GetElementalDamage(out ElementType element, float scaleFactor = 1)
    {
        float fireDamage = offense.fireDamage.GetValue();
        float iceDamage = offense.iceDamage.GetValue();
        float lightningDamage = offense.lightningDamage.GetValue();

        float bonusElementalDamage = major.intelligence.GetValue(); //Danno elementale bonus standard

        float highestDamage = fireDamage;
        element = ElementType.Fire;

        if (iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            element = ElementType.Ice;
        }

        if (lightningDamage > highestDamage)
        {
            highestDamage = lightningDamage;
            element = ElementType.Lightning;
        }

        if (highestDamage <= 0)
        {
            element = ElementType.None;
            return 0;
        }

        float bonusFire = (element == ElementType.Fire) ? 0 : fireDamage * 0.5f;
        float bonusIce = (element == ElementType.Ice) ? 0 : iceDamage * 0.5f;
        float bonusLightning = (element == ElementType.Lightning) ? 0 : lightningDamage * 0.5f;

        float weakerElementalDamage = bonusFire + bonusIce + bonusLightning;
        float finalDamage = highestDamage + weakerElementalDamage + bonusElementalDamage;

        return finalDamage * scaleFactor;
    }

    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = 0f;
        float bonusResistance = major.intelligence.GetValue() * 0.5f; //Bonus Resistenza presa da intelligence: +0.5% per INT

        switch (element)
        {
            case ElementType.Fire:
                baseResistance = defense.fireRes.GetValue();
                break;

            case ElementType.Ice:
                baseResistance = defense.iceRes.GetValue();
                break;

            case ElementType.Lightning:
                baseResistance = defense.lightningRes.GetValue();
                break;
        }

        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 75f; //Massima resistenza consentita
        float finalResistance = Mathf.Clamp(resistance, 0, resistanceCap) / 100; //Converte il valore ad un moltiplicatore da 0 a 1

        return finalResistance;
    }

    public float GetPhysicalDamage(out bool isCrit, float scaleFactor = 1)
    {
        float baseDamage = offense.damage.GetValue();
        float bonusDamage = major.strength.GetValue();
        float totalBaseDamage = baseDamage + bonusDamage;

        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue() * 0.3f; //Bonus Crit Change presa da Agility: + 0.3% per AGI.
        float critChance = baseCritChance + bonusCritChance;

        float baseCritPower = offense.critPower.GetValue();
        float bonusCritPower = major.strength.GetValue() * 0.5f; //Bonus crit chance presa da Strength: + 0.5% per STR
        float critPower = (baseCritPower + bonusCritPower) / 100; //Total crit power come danno aumentato (es: 150 / 100 = 1.5 danno moltiplicato)

        isCrit = Random.Range(0, 100) < critChance;

        float finalDamage = isCrit ? totalBaseDamage * critPower : totalBaseDamage;

        return finalDamage * scaleFactor;
    }

    public float GetMaxHealth()
    {
        float baseMaxHealth = resource.maxHealth.GetValue();
        float bonusMaxHealth = major.vitality.GetValue() * 5;

        float finalMaxHealth = baseMaxHealth + bonusMaxHealth;

        return finalMaxHealth;
    }

    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = major.vitality.GetValue(); //non serve moltiplicarlo
        float totalArmor = baseArmor + bonusArmor;

        float reductionMultiplier = Mathf.Clamp(1 - armorReduction, 0, 1);
        float effectiveArmor = totalArmor * reductionMultiplier;

        float mitigation = totalArmor / (effectiveArmor + 100);
        float mitigationCap = 0.85f; //Mitigation max quindi arriverò a 85%

        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }

    public float GetArmorReduction()
    {
        float finalReduction = offense.armorReduction.GetValue() / 100;

        return finalReduction;
    }

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * 0.5f;

        float totalEvasion = baseEvasion + bonusEvasion; //Evasione totale dello script

        float evasionCap = 85f; //Evasione massima consentita

        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap); //Range evasione

        return finalEvasion;
    }

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            // Resources
            case StatType.MaxHealth: return resource.maxHealth;
            case StatType.HealthRegen: return resource.healthRegen;

            //Major
            case StatType.Strength: return major.strength;
            case StatType.Agility: return major.agility;
            case StatType.Intelligence: return major.intelligence;
            case StatType.Vitality: return major.vitality;

            //Physical Offense
            case StatType.AttackSpeed: return offense.attackSpeed;
            case StatType.Damage: return offense.damage;
            case StatType.CritChance: return offense.critChance;
            case StatType.CritPower: return offense.critPower;
            case StatType.ArmorReduction: return offense.armorReduction;

            //Magical Offense
            case StatType.IceDamage: return offense.iceDamage;
            case StatType.FireDamage: return offense.fireDamage;
            case StatType.LightningDamage: return offense.lightningDamage;

            //Physical Defense
            case StatType.Armor: return defense.armor;
            case StatType.Evasion: return defense.evasion;

            //Magical Defense
            case StatType.IceResistance: return defense.iceRes;
            case StatType.FireResistance: return defense.fireRes;
            case StatType.LightningResistance: return defense.lightningRes;

            default:
                Debug.LogWarning($"Stat_Type {type} non implementato.");
                return null;
        }
    }


    [ContextMenu("Update Default Stat Setup")] //per aggiornare lo stato
    public void ApplyDefaultStatSetup()
    {
        if (defaultStatSetup == null)
        {
            Debug.Log("No default stat setup assegnato");
            return;
        }

        resource.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
        resource.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);

        major.strength.SetBaseValue(defaultStatSetup.strength);
        major.agility.SetBaseValue(defaultStatSetup.agility);
        major.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        major.vitality.SetBaseValue(defaultStatSetup.vitality);

        offense.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
        offense.damage.SetBaseValue(defaultStatSetup.damage);
        offense.critChance.SetBaseValue(defaultStatSetup.critChance);
        offense.critPower.SetBaseValue(defaultStatSetup.critPower);
        offense.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);

        offense.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
        offense.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
        offense.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);

        defense.armor.SetBaseValue(defaultStatSetup.armor);
        defense.evasion.SetBaseValue(defaultStatSetup.evasion);

        defense.iceRes.SetBaseValue(defaultStatSetup.iceResistance);
        defense.fireRes.SetBaseValue(defaultStatSetup.fireResistance);
        defense.lightningRes.SetBaseValue(defaultStatSetup.lightningResistance);
    }
}
