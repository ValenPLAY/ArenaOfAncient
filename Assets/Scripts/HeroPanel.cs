using System;
using TMPro;
using UnityEngine;

public class HeroPanel : MonoBehaviour
{
    [Header("Upgrade Fields")]

    [SerializeField] TMP_Text heroInfoText;


    Hero upgradingUnit;

    [Header("Upgrades and Modifiers")]
    [Header("Research Points")]
    [SerializeField] TMP_Text researchPointsTMP;

    [Header("Health")]
    //Health
    [SerializeField] TMP_Text healthCurrentTMP;
    [SerializeField] float upgradeHealthPercentageBonus;
    [SerializeField] float upgradeHealthFlatBonus;
    //Health Regeneration
    [SerializeField] TMP_Text healthRegenCurrentTMP;
    [SerializeField] float upgradeHealthRegenerationFlatBonus;

    [Header("Damage")]
    //Damage
    [SerializeField] TMP_Text damageCurrentTMP;
    [SerializeField] float upgradeDamagePercentageBonus;
    [SerializeField] float upgradeDamageFlatBonus;
    //Attack Speed
    [SerializeField] TMP_Text attackSpeedCurrentTMP;
    [SerializeField] float attackSpeedPercentageBonus;

    [Header("Energy")]
    //Energy
    [SerializeField] TMP_Text energyCurrentTMP;
    [SerializeField] float upgradeEnergyPercentageBonus;
    [SerializeField] float upgradeEnergyFlatBonus;
    [SerializeField] float abilityDamagePercentageBonus;
    //Energy Regen
    [SerializeField] TMP_Text energyRegenCurrentTMP;
    [SerializeField] float upgradeEnergyRegenFlatBonus;

    [Header("Miscelanious")]
    [SerializeField] TMP_Text armorCurrentTMP;
    [SerializeField] float upgradeArmorFlatBonus;
    [SerializeField] TMP_Text movementSpeedCurrentTMP;
    [SerializeField] float movementSpeedFlatBonus;

    [Header("Additional")]
    [SerializeField] AudioClip onUpgradeSoundEffect;
    [SerializeField] SpecialEffect onUpgradeSpecialEffect;

    private void Awake()
    {
        gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        RefreshInfo();
    }

    public void RefreshInfo()
    {
        if (GameController.Instance != null)
        {
            upgradingUnit = GameController.Instance.selectedHero;
            if (upgradingUnit != null)
            {
                heroInfoText.text = "<color=#ff7d19>" + upgradingUnit.unitName + "</color><br>" + upgradingUnit.unitDescription;
                researchPointsTMP.text = "Current research points: <b>" + upgradingUnit.upgradePoints + "</b>";
                //Health
                healthCurrentTMP.text = upgradingUnit.Health + "";
                healthRegenCurrentTMP.text = upgradingUnit.HealthRegeneration + "/sec";

                //Damage
                damageCurrentTMP.text = MathF.Round(upgradingUnit.Damage, 2) + "";
                attackSpeedCurrentTMP.text = MathF.Round(upgradingUnit.AttackSpeed, 2) + "";

                //Energy
                energyCurrentTMP.text = upgradingUnit.Energy + "";
                energyRegenCurrentTMP.text = upgradingUnit.EnergyRegen + "";


                //Misc
                armorCurrentTMP.text = upgradingUnit.Armor + "";
                movementSpeedCurrentTMP.text = upgradingUnit.MovementSpeed + "";

            }
        }
    }

    void ApplyUpgradeEnd()
    {
        if (onUpgradeSoundEffect != null)
        {
            SoundController.Instance.SpawnSoundEffect(onUpgradeSoundEffect, upgradingUnit.transform.position);
        }

        if (onUpgradeSpecialEffect != null)
        {
            Instantiate(onUpgradeSpecialEffect, upgradingUnit.transform.position, Quaternion.identity);
        }

        upgradingUnit.upgradePoints--;
        RefreshInfo();
    }

    public void UpgradeDamage()
    {
        if (upgradingUnit.upgradePoints > 0)
        {
            upgradingUnit.Damage *= 1.0f + upgradeDamagePercentageBonus;
            upgradingUnit.Damage += upgradeDamageFlatBonus;
            ApplyUpgradeEnd();
        }
    }

    public void UpgradeAttackSpeed()
    {
        if (upgradingUnit.upgradePoints > 0)
        {
            upgradingUnit.AttackSpeed *= 1 - attackSpeedPercentageBonus;
            ApplyUpgradeEnd();
        }
    }

    public void UpgradeHealth()
    {
        if (upgradingUnit.upgradePoints > 0)
        {
            upgradingUnit.Health *= 1.0f + upgradeHealthPercentageBonus;
            upgradingUnit.Health += upgradeHealthFlatBonus;
            ApplyUpgradeEnd();
        }
    }

    public void UpgradeEnergy()
    {
        if (upgradingUnit.upgradePoints > 0)
        {
            upgradingUnit.Energy *= 1.0f + upgradeEnergyPercentageBonus;
            upgradingUnit.Energy += upgradeEnergyFlatBonus;
            ApplyUpgradeEnd();
        }
    }

    public void UpgradeEnergyRegen()
    {
        if (upgradingUnit.upgradePoints > 0)
        {
            upgradingUnit.EnergyRegen += upgradeEnergyRegenFlatBonus;
            ApplyUpgradeEnd();
        }
    }

    public void UpgradeHealthRegen()
    {
        if (upgradingUnit.upgradePoints > 0)
        {
            upgradingUnit.HealthRegeneration += upgradeHealthRegenerationFlatBonus;
            ApplyUpgradeEnd();
        }
    }

    public void UpgradeArmor()
    {
        if (upgradingUnit.upgradePoints > 0)
        {
            upgradingUnit.Armor += upgradeArmorFlatBonus;
            ApplyUpgradeEnd();
        }
    }

    public void UpgradeMovementSpeed()
    {
        if (upgradingUnit.upgradePoints > 0)
        {
            upgradingUnit.MovementSpeed += movementSpeedFlatBonus;
            ApplyUpgradeEnd();
        }
    }
}
