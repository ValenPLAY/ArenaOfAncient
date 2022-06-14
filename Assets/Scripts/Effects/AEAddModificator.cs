using UnityEngine;

public class AEAddModificator : AbilityEffect
{
    //[SerializeField] bool isAddBuffToTarget;
    [SerializeField] Modificator appliedModificator;
    private Modificator createdModificator;

    public override void ApplyEffect(Ability outcomingAbility)
    {
        base.ApplyEffect(outcomingAbility);
        if (appliedModificator == null)
        {
            Debug.Log("Ability modificator is empty");
            return;
        }
        if (!isRequireTarget)
        {
            createdModificator = SpawnController.Instance.CreateModificator(appliedModificator, outcomingAbility.owner);
        }
        else
        {
            if (GameController.Instance.TargetUnit != null)
            {
                createdModificator = SpawnController.Instance.CreateModificator(appliedModificator, GameController.Instance.TargetUnit);
            }
        }

        //if (createdModificator != null)
        //{
        //    if (createdModificator.isDurationAffectedByAnAbility)
        //    {
        //        createdModificator.Duration = outcomingAbility.createdEffectDuration;
        //    }
        //}
    }
}
