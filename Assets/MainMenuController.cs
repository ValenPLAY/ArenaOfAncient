using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : Singleton<MainMenuController>
{
    [Header("Panel Settings")]
    public List<GameObject> mainMenuPanels = new List<GameObject>();
    [SerializeField] int startingPanelID;

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
    [SerializeField] TMP_Text heroDescriptionText;

    [Header("Loading")]
    public Slider loadingBarSlider;
    public LoadingController loadingController;


    private void Awake()
    {
        Time.timeScale = 1;

        if (LoadingController.Instance == null)
        {
            Instantiate(loadingController);
        }

        mainCameraTransform = Camera.main.transform;
        ChangePanel(startingPanelID);
        CreateHeroIcons();
        CreateHero(0);



        if (LoadingController.Instance.isPostGameSummary)
        {
            ChangePanel(3);
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
            heroDescriptionText.text = "<color=#ff7d19><b>" + currentlySelectedHero.unitName + "</b></color><br>" + currentlySelectedHero.unitDescription;
            LoadingController.Instance.loadingHero = selectableHeroes[selectedHeroID];
        }

    }

    public void ChangePanel(int panelID)
    {
        if (panelID < mainMenuPanels.Count)
        {
            for (int x = 0; x < mainMenuPanels.Count; x++)
            {
                mainMenuPanels[x].SetActive(false);
            }
            mainMenuPanels[panelID].SetActive(true);
        }
        else
        {
            Debug.LogWarning("Desired Panel ID is exceeding List limits. Ignoring.");
        }

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


}
