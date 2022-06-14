using UnityEngine;

public class Modificator : MonoBehaviour
{

    [Header("Modificator Settings")]
    [Header("Health")]
    [SerializeField] float healthBonus;
    [SerializeField] float healthRegenBonus;
    [SerializeField] float armorBonus;
    [Header("Damage")]
    [SerializeField] float damageBonus;
    [SerializeField] float attackSpeedBonus;
    [Header("Energy")]
    [SerializeField] float energyBonus;
    [SerializeField] float energyRegenBonus;
    [Header("Miscelanious")]
    [SerializeField] float movementSpeedBonus;
    [SerializeField] float attackRangeBonus;

    private int stateValue;
    private Unit modificatorOwner;

    private void Awake()
    {
        modificatorOwner = transform.parent.GetComponent<Unit>();
        ApplyModificator(modificatorOwner, true);
    }

    public virtual void ApplyModificator(Unit modificatorOwner, bool state)
    {
        if (modificatorOwner == null) return;
        if (modificatorOwner.isAlive == false) return;

        if (state)
        {
            stateValue = 1;
        }
        else
        {
            stateValue = -1;
        }

        if (healthBonus != 0) modificatorOwner.HealthBonus += healthBonus * stateValue;
        if (healthRegenBonus != 0) modificatorOwner.HealthRegenerationBonus += healthRegenBonus * stateValue;
        if (armorBonus != 0) modificatorOwner.ArmorBonus += armorBonus * stateValue;
        if (damageBonus != 0) modificatorOwner.DamageBonus += damageBonus * stateValue;
        if (attackSpeedBonus != 0) modificatorOwner.AttackSpeedBonus += attackSpeedBonus * stateValue;
        if (energyBonus != 0) modificatorOwner.EnergyBonus += energyBonus * stateValue;
        if (energyRegenBonus != 0) modificatorOwner.EnergyRegenBonus += energyRegenBonus * stateValue;
        if (movementSpeedBonus != 0) modificatorOwner.MovementSpeed += movementSpeedBonus * stateValue;
    }

    private void OnDestroy()
    {
        ApplyModificator(modificatorOwner, false);
    }
}
