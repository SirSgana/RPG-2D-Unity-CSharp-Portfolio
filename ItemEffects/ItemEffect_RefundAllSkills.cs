using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Refund All Skills Effect", fileName = "Item Effect Data - Refun All Skills ")]

public class ItemEffect_RefundAllSkills : Item_EffectDataSO
{
    public override void ExecuteEffect()
    {
        UI ui = FindAnyObjectByType<UI>();
        ui.skillTreeUI.RefundAllSkills();
    }
}
