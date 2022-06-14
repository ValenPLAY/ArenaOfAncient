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
        if (correspondingAbility.isActive == false) hotkeyTMP.gameObject.SetActive(false);
        //Input.
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
        abilityDescText.text = "<color=#ff7d19>" + correspondingAbility.getAbilityName() + "</color><br><br>";

        if (correspondingAbility.cooldownDuration > 0)
        {
            abilityDescText.text += "<color=#E1D7CA>Cooldown: " + correspondingAbility.cooldownDuration + " sec</color><br>";
        }
        else
        {
            abilityDescText.text += "<color=#E1D7CA>Cooldown: Instant</color><br>";
        }

        if (correspondingAbility.energyCost > 0)
        {
            abilityDescText.text += "<color=#E1D7CA>Energy Cost: " + correspondingAbility.energyCost + "</color><br>";
        }

        abilityDescText.text += correspondingAbility.getAbilityDesc();
    }

    public void HideDescPanel()
    {
        abilityDescPanel.SetActive(false);
    }
}
