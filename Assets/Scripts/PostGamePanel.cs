using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostGamePanel : MonoBehaviour
{
    [Header("Player EXP")]
    [SerializeField] Slider currentPlayerExpSlider;
    [SerializeField] TMP_Text currentPlayerLevelTMP;
    [SerializeField] TMP_Text expLeftValueTMP;

    [Header("Summary Values")]
    [SerializeField] TMP_Text enemiesKilledTMP;
    [SerializeField] TMP_Text highestWaveTMP;
    [SerializeField] TMP_Text experienceGainedTMP;

    private void OnEnable()
    {
        PlayerProfile.Instance.FinishRun();

        enemiesKilledTMP.text = "Enemies killed: " + PlayerProfile.Instance.finishedGameProgress.EnemiesKilled;
        highestWaveTMP.text = "Highest Wave: " + PlayerProfile.Instance.finishedGameProgress.WavesSurvived;
        experienceGainedTMP.text = "Experience Gained: " + PlayerProfile.Instance.finishedGameProgress.experienceGained;

        currentPlayerLevelTMP.text = PlayerProfile.Instance.playerLevel.ToString();
        expLeftValueTMP.text = PlayerProfile.Instance.Experience + " / " + PlayerProfile.Instance.ExperienceTillNextLevel;
        currentPlayerExpSlider.value = PlayerProfile.Instance.Experience / PlayerProfile.Instance.ExperienceTillNextLevel;
        Debug.Log(PlayerProfile.Instance.Experience / PlayerProfile.Instance.ExperienceTillNextLevel);
    }
}
