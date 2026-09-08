using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Default Stat setup", fileName = "Default Stat Setup")]

public class Stat_SetupSO : ScriptableObject
{
    [Header("Resources")]
    public float maxHealth = 100f;
    public float healthRegen;

    [Header("Offense - Physical Damage")]
    public float attackSpeed = 1f;
    public float damage = 10f;
    public float critChance;
    public float critPower = 150;
    public float armorReduction;

    [Header("Offense - Elemental Damage")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    [Header("Defense - Physical Damage")]
    public float armor;
    public float evasion;

    [Header("Defense - Elemental Damage")]
    public float fireResistance;
    public float iceResistance;
    public float lightningResistance;

    [Header("Major Stats")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;
}
