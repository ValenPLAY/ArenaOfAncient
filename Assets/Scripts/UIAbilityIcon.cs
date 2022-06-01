using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilityIcon : MonoBehaviour
{
    [SerializeField] Image abilityImage;
    [SerializeField] GameObject abilityDescPanel;
    [SerializeField] TMP_Text abilityDescText;
    [SerializeField] TMP_Text hotkeyTMP;
    Ability correspondingAbility;

    [Header("Overlays")]
    [SerializeField] Image notEnoughEnergyOverlay;
    [SerializeField] Image cooldownOverlay;

    public Action showDescriptionPanel;

    private void Awake()
    {
        if (notEnoughEnergyOverlay != null)
        {
            notEnoughEnergyOverlay.gameObject.SetActive(false);
        }

    }

    private void UpdateIconCooldown(float percentage)
    {
        if (cooldownOverlay != null)
        {
            cooldownOverlay.fillAmount = percentage;
        }

    }

    public void ConnectIconToAbility(Ability connectingAbility)
    {
        correspondingAbility = connectingAbility;
        correspondingAbility.onCooldownPercentageChange += UpdateIconCooldown;
    }

    public void UpdateUIIcon(Sprite sprite, int abilityID)
    {
        abilityImage.sprite = sprite;
    }

    public void UpdateAbilityDescription(string abilityName, string abilityDescription)
    {

    }

    public void ShowDescPanel()
    {
        abilityDescPanel.SetActive(true);
        abilityDescText.text = "<color=#ff7d19>" + correspondingAbility.getAbilityName() + "</color><br><br>" + correspondingAbility.getAbilityDesc();
    }

    public void HideDescPanel()
    {
        abilityDescPanel.SetActive(false);
    }
}
