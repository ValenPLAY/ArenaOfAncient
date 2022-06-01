using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : Singleton<LoadingController>
{
    public Hero loadingHero;
    public int essenseAmount;
    
    [Header("Post Game Summary")]
    public bool isPostGameSummary;
    public int wavesSurvived;
    public int enemiesKilled;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadLevelAsync(int sceneID)
    {
        StartCoroutine(LoadAsync(sceneID));
    }

    IEnumerator LoadAsync(int sceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);

        while (!operation.isDone)
        {
            float loadProgress = Mathf.Clamp01(operation.progress / 0.9f);
            if (MainMenuController.Instance != null)
            {
                MainMenuController.Instance.loadingBarSlider.value = loadProgress;
            }
            

            yield return null;
        }
    }

    public void LoadLevel(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
