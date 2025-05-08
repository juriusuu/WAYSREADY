using UnityEngine;
using UnityEngine.UI;

public class StageButtonActivator : MonoBehaviour
{
    [System.Serializable]
    public class StageButton
    {
        public Button button; // The button to activate
        public string[] requiredStages; // The stages that must be completed to activate this button
    }

    public StageButton[] stageButtons; // Array of buttons and their required stages

    private void Start()
    {
        UpdateButtonStates();
    }

    public void UpdateButtonStates()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null! Unable to update button states.");
            return;
        }

        foreach (StageButton stageButton in stageButtons)
        {
            if (stageButton.button == null || stageButton.requiredStages == null || stageButton.requiredStages.Length == 0)
            {
                Debug.LogWarning("Button or required stages are not assigned in the Inspector.");
                continue;
            }

            // Check if all required stages are completed
            bool allStagesCompleted = true;
            foreach (string requiredStage in stageButton.requiredStages)
            {
                if (!GameManager.Instance.completedScenes.Contains(requiredStage))
                {
                    allStagesCompleted = false;
                    break;
                }
            }

            // Enable the button if all required stages are completed
            stageButton.button.interactable = allStagesCompleted;

            Debug.Log($"Button for stages '{string.Join(", ", stageButton.requiredStages)}' is interactable: {allStagesCompleted}");
        }
    }


    public void ResetTrophies()
    {
        // Deactivate all trophy buttons
        foreach (StageButton stageButton in stageButtons)
        {
            if (stageButton.button != null)
            {
                stageButton.button.interactable = false;
            }
        }

        Debug.Log("All trophies have been reset.");
    }
}