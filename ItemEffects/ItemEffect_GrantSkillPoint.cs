using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Grand Skill Point", fileName = "Item Effect Data - Grant Skill Point ")]

public class ItemEffect_GrantSkillPoint : Item_EffectDataSO
{
    [SerializeField] private int pointsToAdd;

    public override void ExecuteEffect()
    {
        UI ui = FindAnyObjectByType<UI>();
        ui.skillTreeUI.AddSkillPoints(pointsToAdd);
    }
}
