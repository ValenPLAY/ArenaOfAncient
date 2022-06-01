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
    public TMP_Text waveNumberText;

    [Header("UI Elements")]

    public Slider healthBar;
    public Slider energyBar;
    private float defaultHealthBarSize;
    private float defaultEnergyBarSize;
    [SerializeField] Image heroImage;
    public HeroPanelButton heroPanelButton;

    public UIInfoPanel uiInfoPanel;

    [Header("Abilities")]
    public GridLayoutGroup abilitiesContainer;

    [Header("Used Prefabs")]
    public UIAbilityIcon basicAbilityIcon;
    public Sprite missingIcon;
    //public uiBuffIcon

    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.onHeroChangeEvent += HeroIconUpdate;
        GameController.Instance.selectedHero.onHealthChangedPercentageEvent += OnHealthChangedCallback;
        GameController.Instance.selectedHero.onCurrentEnergyChangePercentageEvent += OnEnergyChangedCallback;
        GameController.Instance.currentWaveChangedEvent += OnWaveChangedCallback;

        HeroIconUpdate(GameController.Instance.selectedHero);
    }

    // Update is called once per frame
    void Update()
    {

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

    void OnWaveChangedCallback(int currentWave)
    {
        waveNumberText.text = currentWave + "";
    }

    void OnHealthChangedCallback(float healthPercentage)
    {
        //float healthValueActual = (currentHealth / maxHealth);
        healthBar.value = healthPercentage;
    }

    void OnEnergyChangedCallback(float energyPercentage)
    {
        energyBar.value = energyPercentage;
    }

    public void ShowPauseMenu(bool state)
    {
        inGamePanel.SetActive(!state);
        pausePanel.SetActive(state);
    }

}
