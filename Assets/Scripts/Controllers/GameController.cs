using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [System.Flags]
    public enum gameState
    {
        inGame = 1,
        paused = 2,
        betweenWave = 4,
        defeat = 8,
    }

    public gameState currentGameState = gameState.inGame;
    gameState previousGameState;

    public gameState blockedUserInputStates;

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
    public int currentWave = 0;
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

    private float waveProgressPercentage;
    public float WaveProgressPercentage
    {
        get => waveProgressPercentage;
        set
        {
            waveProgressPercentage = value;
            onWaveProgressPercentageChange?.Invoke(waveProgressPercentage);
        }
    }

    public Action<float> onWaveProgressPercentageChange;

    public float difficulty = 1.0f;
    public List<Unit> enemiesOnMap = new List<Unit>();
    //public List<Unit> enemiesToSpawn = new List<Unit>();

    [SerializeField] List<WaveBehaviour> waveBehaviours = new List<WaveBehaviour>();
    [SerializeField] WaveBehaviour currentWaveBehaviour;

    //public Unit waveEnemy;

    public int enemiesPerWave = 1;
    public int enemiesStartCount = 1;

    private int enemiesToSpawnNumber;
    private int enemiesInThisWave;
    private int EnemiesInThisWave
    {
        get => enemiesInThisWave;
        set
        {
            enemiesInThisWave = value;
            enemiesToSpawnNumber = enemiesInThisWave;
            //WaveProgressPercentage = 0.0f;
        }
    }

    [SerializeField] float inBetweenWaveTimer = 5.0f;
    private float inBetweenWaveTimerCurrent;

    [Header("Enemy Spawn Variables")]
    public float inBetweenEnemyTimer = 3.0f;
    private float inBetweenEnemyTimerCurrent;
    private Vector3 spawnPoint;

    [Header("Selection Variables")]
    private Unit lookingAtUnit;
    private Unit targetUnit;
    public Unit TargetUnit
    {
        get => targetUnit;
        set
        {
            targetUnit = value;
            //Debug.Log("Target Unit " + targetUnit);
            onTargetSetEvent?.Invoke(targetUnit);
        }
    }
    public Action<Unit> onTargetSetEvent;

    [Header("Prefab Variables")]
    public Arena arena;
    public InfoPanel infoPanelPrefab;
    //public Transform heroSpawnPoint;
    //public AudioSource audioSourcePrefab;


    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main;

        if (SelectedHero == null)
        {
            SelectedHero = FindObjectOfType<Hero>();
        }

        SpawnHero(arena.spawnPoint.position);
        PlayerProfileUpdate();

        if (arena == null)
        {
            arena = GameObject.FindGameObjectWithTag("Arena").GetComponent<Arena>();
        }

        if (SelectedHero != null)
        {
            SelectedHero.onUnitDeathEvent += DeathEndgame;
        }

        //EnemiesInThisWave = enemiesStartCount;


    }

    void PlayerProfileUpdate()
    {
        if (PlayerProfile.Instance == null) return;
        PlayerProfile.Instance.currentGameProgress = new PlayerProfile.InGameStatistic();
        PlayerProfile.Instance.currentGameProgress.isPostGameSummary = true;
        //Debug.Log(PlayerProfile.Instance.currentGameProgress.isPostGameSummary = true);
    }


    // Update is called once per frame
    void Update()
    {
        PlayerCameraUpdate();
        if (selectedHero != null)
        {
            PlayerInput();
        }

        RollEnemySpawn();
        if (currentGameState == gameState.inGame)
        {
            if (enemiesOnMap.Count + enemiesToSpawnNumber == 0)
            {
                EndWave();
            }

        }

        // Next Wave Check
        if (currentGameState == gameState.betweenWave)
        {
            if (inBetweenWaveTimerCurrent <= 0)
            {
                NextWave();
            }
            else
            {
                inBetweenWaveTimerCurrent -= Time.deltaTime;
                //Debug.Log(inBetweenWaveTimerCurrent / inBetweenWaveTimer);
                WaveProgressPercentage = inBetweenWaveTimerCurrent / inBetweenWaveTimer;
            }
        }

        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.4f);
    }

    void RollEnemySpawn()
    {
        if (currentGameState != gameState.inGame) return;

        if (enemiesToSpawnNumber > 0)
        {
            if (inBetweenEnemyTimerCurrent <= 0)
            {
                Unit spawnedEnemy = SpawnController.Instance.SpawnWaveEnemy(currentWaveBehaviour.GetUnitToSpawn());
                if (spawnedEnemy != null)
                {
                    spawnedEnemy.onUnitDeathEvent += WaveEnemyDeath;
                    enemiesToSpawnNumber--;
                }
                inBetweenEnemyTimerCurrent = inBetweenEnemyTimer;
            }
            else
            {
                inBetweenEnemyTimerCurrent -= Time.deltaTime;

            }

        }
    }

    void WaveEnemyDeath(Unit dyingWaveEnemy)
    {
        dyingWaveEnemy.onUnitDeathEvent -= WaveEnemyDeath;
        enemiesOnMap.Remove(dyingWaveEnemy);
        WaveProgressPercentage = 1 - ((float)enemiesOnMap.Count + (float)enemiesToSpawnNumber) / (float)EnemiesInThisWave;

        PlayerProfile.Instance.currentGameProgress.EnemiesKilled++;
    }

    void SpawnHero(Vector3 spawnPosition)
    {
        if (LoadingController.Instance == null) return;
        if (LoadingController.Instance.loadingHero == null) return;
        if (SelectedHero != null) Destroy(SelectedHero.gameObject);
        SelectedHero = Instantiate(LoadingController.Instance.loadingHero, spawnPosition, Quaternion.identity);
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

        currentWaveBehaviour = GetWaveBehaviour();

        EnemiesInThisWave = enemiesStartCount + (currentWave * enemiesPerWave);
        Debug.Log("Wave " + currentWave + " began!");

    }

    WaveBehaviour GetWaveBehaviour()
    {
        if (waveBehaviours.Count == 0) return null;

        WaveBehaviour selectedWaveBehaviour;
        List<WaveBehaviour> acceptableWaveBehaviours = new List<WaveBehaviour>();

        for (int i = 0; i < waveBehaviours.Count; i++)
        {
            if (waveBehaviours[i].minimumWaveToUseBehaviour < CurrentWave &&
                (waveBehaviours[i].maximumWaveToUseBehaviour > CurrentWave ||
                waveBehaviours[i].maximumWaveToUseBehaviour == 0) &&
                waveBehaviours[i].isSpecificWaveSpawn == false)
            {
                acceptableWaveBehaviours.Add(waveBehaviours[i]);
            }

            if (waveBehaviours[i].isSpecificWaveSpawn == true &&
                waveBehaviours[i].specificWaveSpawn == CurrentWave)
            {
                acceptableWaveBehaviours.Add(waveBehaviours[i]);
            }

            if (waveBehaviours[i].isSpecificWaveSpawn == false &&
                waveBehaviours[i].isSpawnEveryXRounds == true &&
                CurrentWave % waveBehaviours[i].specificWaveSpawn == 0)
            {
                acceptableWaveBehaviours.Add(waveBehaviours[i]);
            }
        }

        if (acceptableWaveBehaviours.Count > 0)
        {
            selectedWaveBehaviour = acceptableWaveBehaviours[UnityEngine.Random.Range(0, acceptableWaveBehaviours.Count)];
            return selectedWaveBehaviour;
        }
        else
        {
            return null;
        }
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
                if (lookingAtUnit.CurrentHealth <= 0) lookingAtUnit = null;
            }


            if (lookingAtUnit != null)
            {
                if (TargetUnit != lookingAtUnit)
                {
                    TargetUnit = lookingAtUnit;
                    playerWorldMousePos = TargetUnit.transform.position;
                    //currentInfoPanel = SpawnInfoPanel(targetUnit);
                    //Debug.Log("Looking at: " + TargetUnit.gameObject.name);
                }
                else
                {
                    playerWorldMousePos = TargetUnit.transform.position;
                }
            }

            if (lookingAtUnit == null && TargetUnit != null)
            {
                TargetUnit = null;
            }

            if (lookingAtUnit == null)
            {
                playerWorldMousePos = hit.point;
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
            PlayerUIController.Instance.ShowHeroPanel(true);
        }

        if (Input.GetButtonUp("Show Hero Panel"))
        {
            PlayerUIController.Instance.ShowHeroPanel(false);
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
        //Player.Instance.currentGameProgress.isPostGameSummary = true;
        PlayerProfile.Instance.currentGameProgress.WavesSurvived = currentWave;

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
