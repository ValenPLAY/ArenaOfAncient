using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [System.Flags]
    private enum gameState
    {
        inGame = 1,
        paused = 2,
        betweenWave = 4,
        defeat = 8,
    }

    gameState currentGameState = gameState.inGame;
    gameState previousGameState;

    [SerializeField] gameState blockedUserInputStates;

    [Header("Player Information")]
    public Hero selectedHero;
    public Hero SelectedHero
    {
        get => selectedHero;
        set
        {
            selectedHero = value;
            onHeroChangeEvent?.Invoke(selectedHero);
        }
    }
    public Action<Hero> onHeroChangeEvent;

    public Vector3 playerWorldMousePos;
    public Camera mainCamera;

    public int enemiesKilled;

    [Header("Hero Movement")]
    [SerializeField] private LayerMask targetAqusitionLayers;

    private Vector3 inputVector;
    private Vector3 InputVector
    {
        get => inputVector;
        set
        {
            inputVector = value;
            onPlayerMovementInput?.Invoke(inputVector);
        }
    }
    public Action<Vector3> onPlayerMovementInput;


    [Header("Wave Variables")]
    public int currentWave = 1;
    public int CurrentWave
    {
        get => currentWave;
        set
        {
            currentWave = value;
            currentWaveChangedEvent?.Invoke(currentWave);
        }
    }

    public Action<int> currentWaveChangedEvent;
    public Action waveFinishedEvent;

    public float difficulty = 1.0f;
    public List<Unit> enemiesOnMap = new List<Unit>();
    //public List<Unit> enemiesToSpawn = new List<Unit>();

    public Unit waveEnemy;
    public int enemiesToSpawnNumber;
    public int enemiesPerWave = 1;
    public int enemiesStartCount = 1;

    public float inBetweenWaveTimer = 5.0f;
    private float inBetweenWaveTimerCurrent;

    [Header("Enemy Spawn Variables")]
    public float inBetweenEnemyTimer = 3.0f;
    private float inBetweenEnemyTimerCurrent;
    private Vector3 spawnPoint;

    [Header("Selection Variables")]
    private Unit lookingAtUnit;
    public Unit targetUnit;

    [Header("Prefab Variables")]
    public Arena arena;
    public InfoPanel infoPanelPrefab;
    public Transform heroSpawnPoint;
    private InfoPanel currentInfoPanel;
    //public AudioSource audioSourcePrefab;


    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main;

        if (selectedHero == null)
        {
            selectedHero = FindObjectOfType<Hero>();
        }

        if (LoadingController.Instance != null)
        {
            if (selectedHero != null) Destroy(selectedHero.gameObject);
            SpawnHero(heroSpawnPoint.position);
        }

        if (arena == null)
        {
            arena = GameObject.FindGameObjectWithTag("Arena").GetComponent<Arena>();
        }

        if (selectedHero != null)
        {
            selectedHero.onUnitDeathEvent += DeathEndgame;
        }

        enemiesToSpawnNumber = enemiesStartCount;
    }


    // Update is called once per frame
    void Update()
    {
        PlayerCameraUpdate();
        if (selectedHero != null)
        {
            PlayerInput();
        }



        if (currentGameState == gameState.inGame)
        {
            if (enemiesToSpawnNumber > 0)
            {
                if (inBetweenEnemyTimerCurrent <= 0)
                {
                    SpawnController.Instance.SpawnEnemy(waveEnemy);
                    inBetweenEnemyTimerCurrent = inBetweenEnemyTimer;
                    enemiesToSpawnNumber--;
                }
                else
                {
                    inBetweenEnemyTimerCurrent -= Time.deltaTime;
                }

            }



            if (enemiesOnMap.Count + enemiesToSpawnNumber == 0)
            {
                EndWave();
            }

        }

        // Next Wave Check
        if (currentGameState == gameState.betweenWave)
        {
            inBetweenWaveTimerCurrent -= Time.deltaTime;
            if (inBetweenWaveTimerCurrent <= 0) NextWave();
        }

        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.4f);
    }

    void SpawnHero(Vector3 spawnPosition)
    {
        selectedHero = Instantiate(LoadingController.Instance.loadingHero, spawnPosition, Quaternion.identity);
    }

    //Wave Specifics
    void EndWave()
    {
        Debug.Log("Wave " + currentWave + " ended!");
        currentGameState = gameState.betweenWave;
        inBetweenWaveTimerCurrent = inBetweenWaveTimer;

        waveFinishedEvent?.Invoke();
        //selectedHero.upgradePoints++;

    }



    void NextWave()
    {
        currentGameState = gameState.inGame;
        CurrentWave++;
        enemiesToSpawnNumber = enemiesStartCount + (currentWave * enemiesPerWave);
        Debug.Log("Wave " + currentWave + " began!");

    }

    public void SetWave(int specificWave)
    {
        currentWave = specificWave;
    }

    // Player Input and Camera
    private void PlayerCameraUpdate()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, targetAqusitionLayers))
        {
            lookingAtUnit = hit.collider.gameObject.GetComponent<Unit>();

            if (lookingAtUnit != null)
            {
                if (targetUnit != lookingAtUnit)
                {
                    targetUnit = lookingAtUnit;
                    playerWorldMousePos = targetUnit.transform.position;
                    //currentInfoPanel = SpawnInfoPanel(targetUnit);
                    Debug.Log("Looking at: " + targetUnit.gameObject.name);
                }
                else
                {
                    playerWorldMousePos = targetUnit.transform.position;
                }
            }

            if (lookingAtUnit == null)
            {
                playerWorldMousePos = hit.point;
                targetUnit = null;

                if (currentInfoPanel != null)
                {
                    Destroy(currentInfoPanel.gameObject);
                }
            }

        }
    }

    private void PlayerInput()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            PauseGame();
        }

        if (currentGameState == blockedUserInputStates) return;

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.z = Input.GetAxis("Vertical");
        InputVector = inputVector;

        if (Input.GetButtonDown("Fire1"))
        {
            selectedHero.OrderAttack();
        }

        if (Input.GetButtonDown("Cast Ability 1") && selectedHero.abilities.Count > 0)
        {
            if (selectedHero.abilities[0] != null) selectedHero.abilities[0].AbilityCastDown();
        }

        if (Input.GetButtonDown("Cast Ability 2") && selectedHero.abilities.Count > 1)
        {
            if (selectedHero.abilities[1] != null) selectedHero.abilities[1].AbilityCastDown();
        }

        if (Input.GetButtonDown("Cast Ability 3") && selectedHero.abilities.Count > 2)
        {
            if (selectedHero.abilities[2] != null) selectedHero.abilities[2].AbilityCastDown();
        }

        if (Input.GetButtonDown("Cast Ability 4") && selectedHero.abilities.Count > 3)
        {
            if (selectedHero.abilities[3] != null) selectedHero.abilities[3].AbilityCastDown();
        }

        if (Input.GetButtonDown("Show Hero Panel"))
        {
            PlayerUIController.Instance.heroPanelButton.ShowHeroPanel();
        }

        if (Input.GetButtonUp("Show Hero Panel"))
        {
            PlayerUIController.Instance.heroPanelButton.HideHeroPanel();
        }



    }

    public void PauseGame()
    {
        if (currentGameState == gameState.inGame)
        {
            previousGameState = currentGameState;
            currentGameState = gameState.paused;
            Time.timeScale = 0;
            PlayerUIController.Instance.ShowPauseMenu(true);
        }
        else if (currentGameState == gameState.paused)
        {
            currentGameState = previousGameState;
            Time.timeScale = 1;
            PlayerUIController.Instance.ShowPauseMenu(false);
        }
    }

    private void DeathEndgame(Unit dyingHero)
    {
        PlayerUIController.Instance.defeatPanel.gameObject.SetActive(true);
        currentGameState = gameState.defeat;
        LoadingController.Instance.isPostGameSummary = true;
        LoadingController.Instance.wavesSurvived = currentWave;

    }

    // Info
    private InfoPanel SpawnInfoPanel(Unit incomingUnit)
    {
        if (infoPanelPrefab != null)
        {
            if (currentInfoPanel != null)
            {
                Destroy(currentInfoPanel.gameObject);
            }

            InfoPanel createdInfoPanel = Instantiate(infoPanelPrefab, incomingUnit.transform);
            createdInfoPanel.transform.position = incomingUnit.transform.position;
            createdInfoPanel.transform.parent = incomingUnit.transform;
            return createdInfoPanel;
        }
        else
        {
            return null;
        }
    }


    // Unit Related
    static void HealUnitPercentage(Unit healedUnit, float percentage)
    {
        healedUnit.HealPercentage(percentage);
    }

    static void HealUnit(Unit healedUnit)
    {
        HealUnitPercentage(healedUnit, 1.0f);
    }
}
