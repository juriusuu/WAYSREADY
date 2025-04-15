using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject failPanel; // Reference to the fail panel
    public GameObject finishPanel; // Reference to the finish panel

    public void ShowFailPanel()
    {
        if (failPanel != null)
        {
            failPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
            Debug.Log("Fail panel shown.");
        }
    }

    public void ShowFinishPanel()
    {
        if (finishPanel != null)
        {
            finishPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
            Debug.Log("Finish panel shown.");
        }
    }

    public void HideAllPanels()
    {
        if (failPanel != null)
        {
            failPanel.SetActive(false);
        }

        if (finishPanel != null)
        {
            finishPanel.SetActive(false);
        }

        Time.timeScale = 1f; // Resume the game
        Debug.Log("All panels hidden.");
    }
}