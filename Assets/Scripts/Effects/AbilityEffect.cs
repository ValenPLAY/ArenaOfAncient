using UnityEngine;

public class AbilityEffect : MonoBehaviour
{
    [Header("Global Effect Variables")]
    [SerializeField] protected Unit effectOwner;
    [SerializeField] protected bool isRequireTarget;
    //[SerializeField] protected bool isAppliedOnce;
    //[SerializeField] protected bool isRequire
    public virtual void ApplyEffect(Ability outcomingAbility)
    {
        effectOwner = outcomingAbility.owner;
        if (effectOwner != null) ApplyEffect();
    }

    public virtual void ApplyEffect(Buff outcomingBuff)
    {
        effectOwner = outcomingBuff.owner;
        if (effectOwner != null) ApplyEffect();
    }

    public virtual void ApplyEffect()
    {
        //if (effectOwner == null) return;
    }
}
