using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    //Consente di aver accesso a tutti gli script delle abilità aggiungendo un componente empty "Skill_Holder" come figlio del player 
    //Permettendo così un lavoro più pulito

    public Skill_Dash dash { get; private set; }
    public Skill_Shard shard { get; private set; }
    public Skill_BowAttack bow { get; private set; }
    public Skill_TimeEcho timeEcho { get; private set; }
    public Skill_DomainExpansion domainExpansion { get; private set; }

    private Skill_Base[] allSkills;

    private void Awake()
    {
        dash = GetComponentInChildren<Skill_Dash>();
        shard = GetComponentInChildren<Skill_Shard>();
        bow = GetComponentInChildren<Skill_BowAttack>();
        timeEcho = GetComponentInChildren<Skill_TimeEcho>();
        domainExpansion = GetComponentInChildren<Skill_DomainExpansion>();

        allSkills = GetComponentsInChildren<Skill_Base>();
    }

    public void ReduceAllSkillCooldownBy(float amount)
    {
        foreach(var skill in allSkills) 
            skill.ReduceCooldownBy(amount);
    }

    public Skill_Base GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.Dash: return dash;
            case SkillType.TimeShard: return shard;
            case SkillType.BowAttack: return bow;
            case SkillType.TimeEcho: return timeEcho;
            case SkillType.DomainExpansion: return domainExpansion;

            default:
                Debug.Log($"Skill type {type} not implemented yet");
                return null;
        }
    }
}
