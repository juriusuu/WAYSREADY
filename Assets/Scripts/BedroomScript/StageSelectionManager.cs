
using UnityEngine;
using UnityEngine.UI;

public class StageSelectionManager : MonoBehaviour
{
    public Button[] stageButtons; // Array of buttons for each stage
    public string[] stageNames; // Array of stage names corresponding to the buttons

    private void Start()
    {
        // Initialize button states
        UpdateButtonStates();

        // Assign button click listeners
        for (int i = 0; i < stageButtons.Length; i++)
        {
            if (stageButtons[i] != null)
            {
                int index = i; // Capture the current index for the lambda
                stageButtons[i].onClick.RemoveAllListeners(); // Clear existing listeners
                stageButtons[i].onClick.AddListener(() => OnStageButtonClicked(index));
            }
        }
    }
    /* 
        private void UpdateButtonStates()
        {
            if (stageButtons.Length != stageNames.Length)
            {
                Debug.LogError("Mismatch between stageButtons and stageNames arrays. Please ensure they have the same length.");
                return;
            }

            for (int i = 0; i < stageButtons.Length; i++)
            {
                if (stageButtons[i] == null)
                {
                    Debug.LogError($"Button at index {i} is not assigned in the Inspector.");
                    continue;
                }

                if (i == 0)
                {
                    // Always enable the first stage
                    stageButtons[i].interactable = true;
                }
                else
                {
                    // Enable the button if the previous stage is completed
                    string previousStage = stageNames[i - 1];
                    bool isCompleted = IsStageCompleted(previousStage);
                    stageButtons[i].interactable = isCompleted;
                }
            }
        } */


    private void UpdateButtonStates()
    {
        if (stageButtons.Length != stageNames.Length)
        {
            Debug.LogError("Mismatch between stageButtons and stageNames arrays. Please ensure they have the same length.");
            return;
        }

        for (int i = 0; i < stageButtons.Length; i++)
        {
            if (stageButtons[i] == null)
            {
                Debug.LogError($"Button at index {i} is not assigned in the Inspector.");
                continue;
            }

            // Make all buttons interactable
            stageButtons[i].interactable = true;
        }
    }
    private bool IsStageCompleted(string stageName)
    {
        if (GameManager.Instance != null)
        {
            return GameManager.Instance.completedScenes.Contains(stageName);
        }
        else
        {
            Debug.LogError("GameManager instance is null! Unable to check stage completion.");
            return false;
        }
    }
    private void OnStageButtonClicked(int index)
    {
        string stageName = stageNames[index];
        Debug.Log($"Button {index} clicked. Stage: {stageName}");

        if (stageName.Contains("Easy"))
        {
            // Navigate to the corresponding storyline panel
            string panelName = GetStorylinePanelName(stageName);
            Debug.Log($"Activating panel: {panelName}");

            // Activate the panel UI
            GameObject panel = GameObject.Find(panelName);
            if (panel != null)
            {
                panel.SetActive(true); // Show the panel
            }
            else
            {
                Debug.LogError($"No storyline panel found for stage: {stageName}");
            }
        }
        else
        {
            // Load the actual stage
            if (!string.IsNullOrEmpty(stageName))
            {
                Debug.Log($"Loading stage: {stageName}");
                UnityEngine.SceneManagement.SceneManager.LoadScene(stageName);
            }
            else
            {
                Debug.LogError($"Stage name at index {index} is null or empty!");
            }
        }
    }

    private string GetStorylinePanelName(string stageName)
    {
        switch (stageName)
        {
            case "Stage1Easy":
                return "HydroStageInformation";
            case "Stage2Easy":
                return "GeoStageInformation";
            case "Stage3Easy":
                return "ManMadeStageInformation";
            default:
                return null; // No storyline panel for other stages
        }
    }
    public void ResetProgress()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.completedScenes.Clear(); // Clear all completed scenes
            Debug.Log("All progress has been reset.");
        }
        else
        {
            Debug.LogError("GameManager instance is null! Unable to reset progress.");
        }

        UpdateButtonStates(); // Update button states after resetting
    }
}