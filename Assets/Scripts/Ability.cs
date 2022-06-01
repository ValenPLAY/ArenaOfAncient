using System;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [Header("Description and Visuals")]
    [SerializeField] string objectName = "Unknown Ability";
    [TextArea] [SerializeField] string description = "A description for an unknown ability";
    [SerializeField] protected Sprite icon;


    [Header("Active Parameters")]
    [SerializeField] bool isActive;
    public float energyCost;
    [SerializeField] bool isOnCastUp;

    [Header("Passive Parameters")]
    [SerializeField] bool isAutoCastPassive;

    [Header("Cooldown Parameters")]
    [SerializeField] protected float cooldownDuration;
    protected float cooldownDurationCurrent;
    protected float CooldownDurationCurrent
    {
        get => cooldownDurationCurrent;
        set
        {
            cooldownDurationCurrent = value;
            cooldownDurationPercentage = cooldownDuration != 0 ? (cooldownDurationCurrent / cooldownDuration) : 0;

            onCooldownPercentageChange?.Invoke(cooldownDurationPercentage);
        }
    }
    protected float cooldownDurationPercentage;

    public Action onAbilityCooldownEvent;
    public Action<float> onCooldownPercentageChange;

    public Unit owner;
    protected int number;
    protected UIAbilityIcon correspondingIcon;

    [Header("Effects")]
    [SerializeField] protected List<AbilityEffect> effects = new List<AbilityEffect>();
    //[SerializeField] protected List<Buff> appliedBuffs = new List<Buff>();

    [Serializable]
    public class SpawnableBuff
    {
        public bool isAppliedToTarget;
        public Buff buffToSpawn;
    }

    public List<SpawnableBuff> addableBuffsList = new List<SpawnableBuff>();

    [Header("Effect Modifiers")]
    public float createdEffectDamage;
    public float createdEffectDuration;
    public float createdEffectArea;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        base.name = "Ability: " + objectName;
        owner = transform.parent.GetComponent<Hero>();

        if (owner != null && PlayerUIController.Instance != null)
        {
            correspondingIcon = Instantiate(PlayerUIController.Instance.basicAbilityIcon, PlayerUIController.Instance.abilitiesContainer.transform);

            if (icon != null)
            {
                correspondingIcon.UpdateUIIcon(icon, 0);

                //correspondingIcon.gameObject.SetActive(true);
            }

            correspondingIcon.ConnectIconToAbility(this);
            CooldownDurationCurrent = cooldownDuration;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (CooldownDurationCurrent > 0) CooldownDurationCurrent -= Time.deltaTime;

        if (isAutoCastPassive && CooldownDurationCurrent <= 0)
        {
            CastAbility();
        }
    }

    public virtual void AbilityCastDown()
    {
        if (isActive && !isOnCastUp)
        {
            CastAbility();
        }
    }

    public virtual void AbilityCastUp()
    {
        if (isActive && isOnCastUp)
        {
            CastAbility();
        }
    }

    public virtual void CastAbility()
    {
        if (CooldownDurationCurrent > 0)
        {
            Debug.Log("Ability is on Cooldown");
            return;
        }

        if (energyCost <= owner.Energy)
        {
            foreach (AbilityEffect appliedEffect in effects)
            {
                appliedEffect.ApplyEffect(this);
                Debug.Log("Ability Casted");
            }

            foreach (SpawnableBuff appliedBuff in addableBuffsList)
            {
                if (!appliedBuff.isAppliedToTarget)
                {
                    SpawnController.Instance.CreateBuff(owner, appliedBuff.buffToSpawn);
                }
                else
                {
                    if (GameController.Instance.targetUnit != null)
                    {
                        SpawnController.Instance.CreateBuff(GameController.Instance.targetUnit, appliedBuff.buffToSpawn);
                    }
                }
            }

            CooldownDurationCurrent = cooldownDuration;
            owner.CurrentEnergy -= energyCost;
        }
        else
        {
            Debug.Log("Not enough Energy to cast this Ability");
        }
    }

    protected virtual void OnDestroy()
    {
        if (correspondingIcon != null)
        {
            Destroy(correspondingIcon);
        }
    }

    public virtual string getAbilityName()
    {
        return objectName;
    }

    public virtual string getAbilityDesc()
    {
        return description;
    }
}
