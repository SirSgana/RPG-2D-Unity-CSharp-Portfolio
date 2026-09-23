using UnityEngine;

public class Item_EffectDataSO : ScriptableObject
{
    [TextArea]
    public string effectDescription;
    protected Player player;

    public virtual bool CanBeUse()
    {
        return true;
    }

    public virtual void ExecuteEffect()
    {
    
    }

    public virtual void Subscribe(Player player)
    {
        this.player = player;
    }

    public virtual void Unsubscribe()
    {

    }
}
