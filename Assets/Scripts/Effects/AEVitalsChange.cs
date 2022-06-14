using UnityEngine;

[CreateAssetMenu(fileName = "Effect Vitals Change", menuName = "Ability Effects/Vitals Change")]
public class AEVitalsChange : AbilityEffect
{
    [Header("Health Changer")]
    [SerializeField] float healthFlatAmount;
    [SerializeField] float healthPercentageAmount;
    [Header("Energy Changer")]
    [SerializeField] float energyFlatAmount;
    [SerializeField] float energyPercentageAmount;

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        if (healthFlatAmount > 0) effectOwner.HealFlat(healthFlatAmount);
        if (healthPercentageAmount > 1) effectOwner.HealPercentage(healthPercentageAmount);
        if (energyFlatAmount > 0) effectOwner.CurrentEnergy += energyFlatAmount;
        if (energyPercentageAmount > 1) effectOwner.Energy *= energyPercentageAmount;
    }
}
