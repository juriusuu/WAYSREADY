using UnityEngine;

public class PanelsManager : MonoBehaviour
{
    public static PanelsManager Instance;

    public GameObject stage2StorylinePanel; // Reference to the Stage 2 Storyline panel
    public GameObject stage3StorylinePanel; // Reference to the Stage 3 Storyline panel
    public GameObject congratulationsPanel; // Reference to the Congratulations panel

    private void Awake()
    {
        // Ensure this is the only instance of PanelsManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowPanel(string panelName)
    {
        if (panelName == "Stage 2 Storyline" && stage2StorylinePanel != null)
        {
            stage2StorylinePanel.SetActive(true);
            Debug.Log("Activated Stage 2 Storyline panel.");
        }
        else if (panelName == "Stage 3 Storyline" && stage3StorylinePanel != null)
        {
            stage3StorylinePanel.SetActive(true);
            Debug.Log("Activated Stage 3 Storyline panel.");
        }
        else if (panelName == "Congratulations" && congratulationsPanel != null)
        {
            congratulationsPanel.SetActive(true);
            Debug.Log("Activated Congratulations panel.");
        }
        else
        {
            Debug.LogWarning($"Panel '{panelName}' not found or not assigned.");
        }
    }
}