using UnityEngine;

public class Modificator : MonoBehaviour
{

    [Header("Modificator Settings")]
    //[SerializeField] bool isPermanent;
    //public bool isDurationAffectedByAnAbility;
    //[SerializeField] float duration;
    //public float Duration
    //{
    //    get => duration;
    //    set
    //    {
    //        duration = value;
    //        currentDuration = duration;
    //    }
    //}
    //[SerializeField] float currentDuration;

    [Header("Health")]
    [SerializeField] float healthBonus;
    [SerializeField] float healthRegenBonus;
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

    //private void Update()
    //{
    //    if (!isPermanent && duration > 0)
    //    {
    //        if (currentDuration <= 0)
    //        {
    //            Destroy(gameObject);
    //        }
    //        else
    //        {
    //            currentDuration -= Time.deltaTime;
    //        }
    //    }
    //}

    //[SerializeField] protected bool isRequire
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
