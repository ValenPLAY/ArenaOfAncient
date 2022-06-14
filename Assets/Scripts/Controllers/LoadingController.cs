using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : SingletonAuto<LoadingController>
{
    [Header("Loading")]
    public Hero loadingHero;
    public int levelToLoadID;
    public Action<float> loadingProgressEvent;

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
            loadingProgressEvent?.Invoke(loadProgress);


            yield return null;
        }
    }

    public void LoadLevelLoadingScreen(int sceneID)
    {
        levelToLoadID = sceneID;
        SceneManager.LoadScene(1);
    }

    public void LoadLevel(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
