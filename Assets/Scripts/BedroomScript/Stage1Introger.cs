using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1IntroManager : MonoBehaviour
{
    public GameObject hudmovement; // Assign in Inspector
    public GameObject timerObject; // Assign your timer GameObject (e.g., TaymerManager or UI)
    public GameObject dialogueManager; // Assign your dialogue manager GameObject

    private void Start()
    {
        // Only show in Stage1Easy
        if (SceneManager.GetActiveScene().name == "Stage1Easy")
        {
            ShowHud();
        }
        else
        {
            if (hudmovement != null) hudmovement.SetActive(false);
            if (dialogueManager != null) dialogueManager.SetActive(true);
        }
    }

    private void ShowHud()
    {
        if (hudmovement != null) hudmovement.SetActive(true);
        if (dialogueManager != null) dialogueManager.SetActive(false);
        // timerObject stays visible, but timer script should not start yet
    }

    // Call this from your HUD's continue button
    public void OnContinueClicked()
    {
        if (hudmovement != null) hudmovement.SetActive(false);
        if (dialogueManager != null) dialogueManager.SetActive(true);

        // Now start the timer (replace with your timer script/method)
        timerObject.GetComponent<TaymerManager>()?.StartTimer();
    }
}