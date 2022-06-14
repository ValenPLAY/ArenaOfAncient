using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetPanel : MonoBehaviour
{
    [Header("Target Panel Settings")]
    [SerializeField] Image targetUnitIcon;
    Sprite defaultIconSprite;
    [SerializeField] Slider targetHealthSlider;
    [SerializeField] TMP_Text targetHealthValueTMP;
    [SerializeField] TMP_Text targetNameTMP;
    Unit displayedUnit;

    private void Awake()
    {
        defaultIconSprite = targetUnitIcon.sprite;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (GameController.Instance == null) return;
        displayedUnit = GameController.Instance.TargetUnit;
        UpdatePanel();
    }

    private void OnDisable()
    {
        if (displayedUnit == null) return;
        displayedUnit.onCurrentHealthChangedEvent -= UpdateHealthValue;
        displayedUnit.onHealthChangedPercentageEvent -= UpdateHealthSlider;
    }

    private void UpdatePanel()
    {
        if (displayedUnit == null) return;

        targetNameTMP.text = displayedUnit.name;

        if (displayedUnit.unitIcon != null)
        {
            targetUnitIcon.sprite = displayedUnit.unitIcon;
        }
        else
        {
            targetUnitIcon.sprite = defaultIconSprite;
        }

        UpdateHealthSlider(displayedUnit.currentHealthPercentage);
        UpdateHealthValue(displayedUnit.CurrentHealth);

        displayedUnit.onCurrentHealthChangedEvent += UpdateHealthValue;
        displayedUnit.onHealthChangedPercentageEvent += UpdateHealthSlider;
    }

    private void UpdateHealthSlider(float healthPercentage)
    {
        targetHealthSlider.value = healthPercentage;
    }

    private void UpdateHealthValue(float healthFlat)
    {
        targetHealthValueTMP.text = (int)healthFlat + "";
    }


}
