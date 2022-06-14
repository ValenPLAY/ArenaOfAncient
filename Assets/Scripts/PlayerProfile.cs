using System;
using UnityEngine;

public class PlayerProfile : SingletonAuto<PlayerProfile>
{
    [Header("Player Progression")]
    [SerializeField] private float experience;
    public float Experience
    {
        get => experience;
        set
        {
            experience = value;
            PlayerLevelUpdate();
        }
    }
    [SerializeField] public int playerLevel = 1;
    [SerializeField] private float experiencePerLevel = 100.0f;
    [SerializeField] private float experiencePerLevelMultiplicator = 1.0f;
    private float experienceTillNextLevel;
    public float ExperienceTillNextLevel
    {
        get
        {
            experienceTillNextLevel = playerLevel * experiencePerLevel;
            return experienceTillNextLevel;
        }
        set
        {
            experienceTillNextLevel = value;
        }
    }
    [SerializeField] private int highestWaveSurvived;
    [SerializeField] private int totalEnemiesKilled;

    [Header("Experience Values")]
    public float finishedWaveExperience = 30.0f;
    public float basicInfestedKilledExperience = 1.0f;

    [Serializable]
    public class InGameStatistic
    {
        public bool isPostGameSummary = false;
        private int wavesSurvived;
        public int WavesSurvived
        {
            get => wavesSurvived;
            set
            {
                wavesSurvived = value;
                if (wavesSurvived > PlayerProfile.Instance.highestWaveSurvived)
                {
                    PlayerProfile.Instance.highestWaveSurvived = wavesSurvived;
                }
                experienceGained = (int)((enemiesKilled * PlayerProfile.Instance.basicInfestedKilledExperience) + (WavesSurvived * PlayerProfile.Instance.finishedWaveExperience));
            }
        }
        private int enemiesKilled;
        public int EnemiesKilled
        {
            get => enemiesKilled;
            set
            {
                enemiesKilled = value;
                experienceGained = (int)((enemiesKilled * PlayerProfile.Instance.basicInfestedKilledExperience) + (WavesSurvived * PlayerProfile.Instance.finishedWaveExperience));
            }
        }
        public int experienceGained;
    }

    [Header("In-Game Statistic")]
    [SerializeField] public InGameStatistic currentGameProgress = new InGameStatistic();
    public InGameStatistic finishedGameProgress = new InGameStatistic();

    public void UpdateKillCount(int killedEnemiesNumber)
    {
        totalEnemiesKilled += killedEnemiesNumber;
    }

    public void UpdateHighestWaveSurvived(int wavesSurvived)
    {
        if (wavesSurvived > highestWaveSurvived)
        {
            highestWaveSurvived = wavesSurvived;
        }
    }

    public void FinishRun()
    {
        if (currentGameProgress.isPostGameSummary == false) return;

        finishedGameProgress = currentGameProgress;

        totalEnemiesKilled += currentGameProgress.EnemiesKilled;
        Experience += currentGameProgress.experienceGained;

        currentGameProgress = new InGameStatistic();
    }

    void PlayerLevelUpdate()
    {
        experienceTillNextLevel = playerLevel * experiencePerLevel;
        if (experience > experienceTillNextLevel)
        {
            playerLevel++;
            PlayerLevelUpdate();
        }
    }
}
