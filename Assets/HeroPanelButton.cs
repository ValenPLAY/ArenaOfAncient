using UnityEngine;

public class HeroPanelButton : MonoBehaviour
{
    [SerializeField] GameObject heroPanel;
    // Start is called before the first frame update
    public void ShowHeroPanel()
    {
        heroPanel.SetActive(true);
    }

    public void HideHeroPanel()
    {
        heroPanel.SetActive(false);
    }
}
