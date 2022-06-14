using UnityEngine;

[CreateAssetMenu(fileName = "Effect Add Buff", menuName = "Ability Effects/Add Buff")]
public class AEAddBuff : AbilityEffect
{
    //[SerializeField] bool isAddBuffToTarget;
    [SerializeField] Buff appliedBuff;

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        if (!isRequireTarget)
        {
            SpawnController.Instance.CreateBuff(effectOwner, appliedBuff);
        }
        else
        {
            if (GameController.Instance.TargetUnit != null)
            {
                SpawnController.Instance.CreateBuff(GameController.Instance.TargetUnit, appliedBuff);
            }
        }
    }

}
