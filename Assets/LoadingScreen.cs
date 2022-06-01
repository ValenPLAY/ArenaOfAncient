using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public void StartLoading(int loadID)
    {
        LoadingController.Instance.LoadLevelAsync(loadID);
    }
}
