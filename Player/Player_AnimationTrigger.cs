using UnityEngine;

public class Player_AnimationTrigger : Entity_AnimationTriggers
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<Player>();
    }

    private void ArrowThrow() => player.skillManager.bow.ArrowThrow();
}
