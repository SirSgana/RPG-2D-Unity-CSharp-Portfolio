using System;
using UnityEngine;

[Serializable]

public class Stat_OffenseGroup 
{
    public Stat attackSpeed;

    //Danno Fisico
    public Stat damage;
    public Stat critPower;
    public Stat critChance;
    public Stat armorReduction;

    //Danno Elementale
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
}
