using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuController : Singleton<MainMenuController>
{
    [Header("Panel Settings")]
    [SerializeField] PanelSwitcher mainMenuPanelSwitcher;
    [SerializeField] private int endGamePanelID;

    [Header("Panning and View Points Options")]
    public List<Transform> cameraPoints = new List<Transform>();
    [SerializeField] float cameraPanSpeed = 1.0f;
    private Transform currentViewPoint;
    private Transform mainCameraTransform;

    [Header("Hero Selection Options")]
    [SerializeField] List<Hero> selectableHeroes = new List<Hero>();
    private Hero currentlySelectedHero;
    [SerializeField] Transform defaultHeroPosition;
    private List<HeroSelectionButton> heroSelectionButtons = new List<HeroSelectionButton>();
    [SerializeField] Transform heroSelectionGrid;
    [SerializeField] HeroSelectionButton heroSelectionButtonPrefab;
    [Header("Hero Information Tab")]
    [SerializeField] TMP_Text heroNameTMP;
    [SerializeField] TMP_Text heroDescriptionTMP;

    //[Header("Controllers")]
    //public Slider loadingBarSlider;
    //public LoadingController loadingController;


    private void Awake()
    {
        Time.timeScale = 1;
        mainMenuPanelSwitcher.onPanelChangeEvent += PanCamera;
        mainMenuPanelSwitcher.ChangePanel(0);
        mainCameraTransform = Camera.main.transform;
        CreateHeroIcons();
        CreateHero(0);



        if (PlayerProfile.Instance.currentGameProgress.isPostGameSummary)
        {
            PlayerProfile.Instance.UpdateKillCount(PlayerProfile.Instance.currentGameProgress.EnemiesKilled);
            mainMenuPanelSwitcher.ChangePanel(endGamePanelID);
        }
        else
        {
            mainMenuPanelSwitcher.ChangePanel(0);
        }


    }

    void Update()
    {
        mainCameraTransform.position = Vector3.Lerp(mainCameraTransform.position, currentViewPoint.position, cameraPanSpeed * Time.deltaTime);
        mainCameraTransform.rotation = Quaternion.Lerp(mainCameraTransform.rotation, currentViewPoint.rotation, cameraPanSpeed * Time.deltaTime);

        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.4f);
    }

    void CreateHeroIcons()
    {
        for (int x = 0; x < selectableHeroes.Count; x++)
        {
            HeroSelectionButton createdIcon = Instantiate(heroSelectionButtonPrefab, heroSelectionGrid);
            createdIcon.ChangeIcon(selectableHeroes[x].unitIcon);
            createdIcon.onButtonHeroClick += CreateHero;
            heroSelectionButtons.Add(createdIcon);
            createdIcon.heroButtonID = heroSelectionButtons.Count - 1;
        }
    }

    public void CreateHero(int selectedHeroID)
    {
        if (currentlySelectedHero != null)
        {
            Destroy(currentlySelectedHero.gameObject);
        }
        if (selectableHeroes.Count > 0)
        {
            currentlySelectedHero = Instantiate(selectableHeroes[selectedHeroID], defaultHeroPosition);
            heroNameTMP.text = currentlySelectedHero.unitName;
            heroDescriptionTMP.text = currentlySelectedHero.unitDescription;
            LoadingController.Instance.loadingHero = selectableHeroes[selectedHeroID];
        }

    }

    public void PanCamera(int panelID)
    {
        if (panelID < cameraPoints.Count)
        {
            currentViewPoint = cameraPoints[panelID];
        }
        else
        {
            currentViewPoint = cameraPoints[0];
        }
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void StartGame(int mapID)
    {
        LoadingController.Instance.LoadLevelLoadingScreen(mapID);
    }
}
