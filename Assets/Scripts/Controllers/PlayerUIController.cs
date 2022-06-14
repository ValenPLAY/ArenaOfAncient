using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : Singleton<PlayerUIController>
{
    [Header("Panels")]
    public GameObject inGamePanel;
    public GameObject pausePanel;
    public GameObject defeatPanel;

    [Header("Wave Info Display")]
    [SerializeField] TMP_Text waveNumberText;
    [SerializeField] Slider waveCompletionSlider;
    private float currentWaveValue;
    private float waveProgressUpdateValue;
    private float waveProgressUpdateMultiplier = 0.01f;

    [Header("UI Elements")]
    [Header("Unit Vitals")]
    [SerializeField] Slider healthBar;
    [SerializeField] Slider energyBar;
    [SerializeField] TMP_Text healthValueTMP;
    [SerializeField] TMP_Text energyValueTMP;

    [SerializeField] Image heroImage;
    //public HeroPanelButton heroPanelButton;
    [SerializeField] GameObject heroPanel;

    [Header("Target Panel")]
    [SerializeField] TargetPanel targetPanel;
    [SerializeField] GameObject targetCirclePrefab;
    GameObject currentTargetCircle;
    Vector3 currentTargetCircleScale;

    //public UIInfoPanel uiInfoPanel;

    [Header("Abilities")]
    public GridLayoutGroup abilitiesContainer;

    [Header("Used Prefabs")]
    public UIAbilityIcon basicAbilityIcon;
    public Sprite missingIcon;
    //public uiBuffIcon

    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.onTargetSetEvent += ShowTargetPanel;

        GameController.Instance.onHeroChangeEvent += HeroIconUpdate;

        GameController.Instance.currentWaveChangedEvent += OnWaveChangedCallback;
        OnWaveChangedCallback(GameController.Instance.currentWave);

        GameController.Instance.onWaveProgressPercentageChange += UpdateWaveProgressSlider;

        GameController.Instance.selectedHero.onHealthChangedPercentageEvent += OnHealthPercentageChange;
        OnHealthPercentageChange(GameController.Instance.selectedHero.currentHealthPercentage);

        GameController.Instance.selectedHero.onCurrentHealthChangedEvent += OnHealthValueChange;
        OnHealthValueChange(GameController.Instance.selectedHero.CurrentHealth);

        GameController.Instance.selectedHero.onCurrentEnergyChangePercentageEvent += OnEnergyPercentageChange;
        OnEnergyPercentageChange(GameController.Instance.selectedHero.CurrentEnergy / GameController.Instance.selectedHero.Energy);

        GameController.Instance.selectedHero.onCurrentEnergyChangeEvent += OnEnergyValueChange;
        OnEnergyValueChange(GameController.Instance.selectedHero.CurrentEnergy);

        HeroIconUpdate(GameController.Instance.selectedHero);
    }

    // Update is called once per frame
    void Update()
    {
        if (waveCompletionSlider.value != currentWaveValue)
        {
            waveCompletionSlider.value = Mathf.Lerp(waveCompletionSlider.value, currentWaveValue, waveProgressUpdateValue);
            waveProgressUpdateValue += Time.deltaTime * waveProgressUpdateMultiplier;
        }
        else
        {
            waveProgressUpdateValue = 0;
        }
    }

    private void HeroIconUpdate(Hero changedHero)
    {
        if (changedHero.unitIcon != null)
        {
            heroImage.sprite = changedHero.unitIcon;
        }
        else
        {
            heroImage.sprite = PlayerUIController.Instance.missingIcon;
        }

    }

    void UpdateWaveProgressSlider(float waveProgressPercentage)
    {

        currentWaveValue = waveProgressPercentage;
    }

    void OnWaveChangedCallback(int currentWave)
    {
        waveNumberText.text = currentWave + "";
    }

    void OnHealthPercentageChange(float healthPercentage)
    {
        //float healthValueActual = (currentHealth / maxHealth);
        healthBar.value = healthPercentage;
    }

    void OnHealthValueChange(float healthValue)
    {
        healthValueTMP.text = (int)healthValue + "";
    }

    void OnEnergyPercentageChange(float energyPercentage)
    {
        energyBar.value = energyPercentage;
    }

    void OnEnergyValueChange(float energyValue)
    {
        energyValueTMP.text = (int)energyValue + "";
    }

    public void ShowPauseMenu(bool state)
    {
        inGamePanel.SetActive(!state);
        pausePanel.SetActive(state);
    }

    public void ShowTargetPanel(Unit targetUnit)
    {
        targetPanel.gameObject.SetActive(targetUnit != null);

        if (currentTargetCircle != null)
        {
            Destroy(currentTargetCircle);
        }

        if (targetUnit != null && targetCirclePrefab != null)
        {
            currentTargetCircle = Instantiate(targetCirclePrefab, targetUnit.transform);
            currentTargetCircle.transform.position = targetUnit.UnitGroundPosition;

            currentTargetCircleScale = currentTargetCircle.transform.localScale;
            currentTargetCircleScale.x = targetUnit.UnitWidth;
            currentTargetCircleScale.z = targetUnit.UnitWidth;

            //Debug.Log(targetUnit.UnitWidth);
        }
    }


    private void OnDestroy()
    {
        if (GameController.Instance != null)
        {
            GameController.Instance.onTargetSetEvent -= ShowTargetPanel;

            GameController.Instance.onHeroChangeEvent -= HeroIconUpdate;

            GameController.Instance.currentWaveChangedEvent -= OnWaveChangedCallback;
            GameController.Instance.onWaveProgressPercentageChange -= UpdateWaveProgressSlider;

            GameController.Instance.selectedHero.onHealthChangedPercentageEvent -= OnHealthPercentageChange;
            GameController.Instance.selectedHero.onCurrentHealthChangedEvent -= OnHealthValueChange;


            GameController.Instance.selectedHero.onCurrentEnergyChangePercentageEvent -= OnEnergyPercentageChange;
            GameController.Instance.selectedHero.onCurrentEnergyChangeEvent -= OnEnergyValueChange;
        }
    }

    public void ShowHeroPanel(bool state)
    {
        if (GameController.Instance.currentGameState == GameController.Instance.blockedUserInputStates) return;
        heroPanel.SetActive(state);
    }
}
