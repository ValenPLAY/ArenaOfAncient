using System;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    [Header("Panel Settings")]
    public List<GameObject> mainMenuPanels = new List<GameObject>();
    [SerializeField] bool isApplyStartingPanelOnEnable;
    [SerializeField] int startingPanelID;
    private int currentPanelID;
    private int previousPanelID;

    public Action<int> onPanelChangeEvent;


    private void OnEnable()
    {
        if (isApplyStartingPanelOnEnable)
        {
            ChangePanel(startingPanelID);
        }
    }

    public void ChangePanel(int panelID)
    {
        if (panelID == -1 && currentPanelID != previousPanelID)
        {
            currentPanelID = previousPanelID;
        }
        else
        {
            previousPanelID = currentPanelID;
            currentPanelID = panelID;
        }

        onPanelChangeEvent?.Invoke(currentPanelID);

        if (currentPanelID < mainMenuPanels.Count)
        {
            for (int x = 0; x < mainMenuPanels.Count; x++)
            {
                mainMenuPanels[x].SetActive(false);
            }
            mainMenuPanels[currentPanelID].SetActive(true);
        }
        else
        {
            Debug.LogWarning("Desired Panel ID is exceeding List limits. Ignoring.");
        }
    }
}
