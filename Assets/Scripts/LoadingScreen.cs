using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Slider loadingProgressBar;
    [SerializeField] TMP_Text loadingProgressValue;

    private void Awake()
    {
        LoadingController.Instance.loadingProgressEvent += UpdateLoadingInfo;
    }

    private void Start()
    {
        StartLoading(LoadingController.Instance.levelToLoadID);
    }

    public void StartLoading(int loadID)
    {
        LoadingController.Instance.LoadLevelAsync(loadID);
    }

    void UpdateLoadingInfo(float updateProgress)
    {
        loadingProgressBar.value = updateProgress;
        loadingProgressValue.text = updateProgress + "%";
    }

    private void OnDestroy()
    {
        LoadingController.Instance.loadingProgressEvent -= UpdateLoadingInfo;
    }
}
