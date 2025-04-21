/* using UnityEngine;
using UnityEngine.UI;

public class StageSelectionManager : MonoBehaviour
{
    public Button[] stageButtons; // Array of buttons for each stage
    public string[] stageNames; // Array of stage names corresponding to the buttons

    private void Start()
    {
        UpdateButtonStates();
    }

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
                Debug.Log($"Button {i} ({stageNames[i]}) is interactable: true");
            }
            else
            {
                // Enable the button if the previous stage is completed
                bool isCompleted = IsStageCompleted(stageNames[i - 1]);
                stageButtons[i].interactable = isCompleted;
                Debug.Log($"Button {i} ({stageNames[i]}) is interactable: {isCompleted}");
            }
        }
    } */
/* 
    private bool IsStageCompleted(string stageName)
    {
        int value = PlayerPrefs.GetInt(stageName, 0);
        Debug.Log($"Stage '{stageName}' completion status: {value}");
        return value == 1; // Check if the stage is completed
    } */
/*     private bool IsStageCompleted(string stageName)
    {
        if (GameManager.Instance != null)
        {
            bool isCompleted = GameManager.Instance.completedScenes.Contains(stageName);
            Debug.Log($"Stage '{stageName}' completion status: {isCompleted}");
            return isCompleted;
        }
        else
        {
            Debug.LogError("GameManager instance is null! Unable to check stage completion.");
            return false;
        }
    }
    public void MarkStageAsCompleted(string stageName)
    {
        PlayerPrefs.SetInt(stageName, 1); // Save 1 to indicate the stage is completed
        PlayerPrefs.Save(); // Save PlayerPrefs to persist the data
        Debug.Log($"Stage '{stageName}' marked as completed.");
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll(); // Clear all saved progress
        PlayerPrefs.Save();
        Debug.Log("All progress has been reset.");
        UpdateButtonStates(); // Update button states after resetting
    }
} */

using UnityEngine;
using UnityEngine.UI;

public class StageSelectionManager : MonoBehaviour
{
    public Button[] stageButtons; // Array of buttons for each stage
    public string[] stageNames; // Array of stage names corresponding to the buttons

    private void Start()
    {

        for (int i = 0; i < stageButtons.Length; i++)
        {
            if (stageButtons[i] != null)
            {
                int index = i; // Capture the current index for the lambda
                stageButtons[i].onClick.RemoveAllListeners(); // Clear existing listeners
                stageButtons[i].onClick.AddListener(() => OnStageButtonClicked(index));
            }
        }
        // GameManager.Instance.LoadGame(); // Ensure the game is loaded
        UpdateButtonStates();
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
                    Debug.Log($"Button {i} ({stageNames[i]}) is interactable: true");
                }
                else
                {
                    // Enable the button if the previous stage is completed
                    bool isCompleted = IsStageCompleted(stageNames[i - 1]);
                    //  stageButtons[i].interactable = false; // Temporarily disable
                    stageButtons[i].interactable = isCompleted; // Re-enable based on completion status
                    Debug.Log($"Button {i} ({stageNames[i]}) is interactable: {isCompleted}");
                }
            }
        }
     */
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
            Debug.Log($"Button {i} ({stageNames[i]}) is interactable: true");
        }
    }
    private bool IsStageCompleted(string stageName)
    {
        if (GameManager.Instance != null)
        {
            bool isCompleted = GameManager.Instance.completedScenes.Contains(stageName);
            Debug.Log($"Stage '{stageName}' completion status: {isCompleted}");
            return isCompleted;
        }
        else
        {
            Debug.LogError("GameManager instance is null! Unable to check stage completion.");
            return false;
        }
    }

    private void OnStageButtonClicked(int index)
    {
        Debug.Log($"Button {index} clicked. Loading scene: {stageNames[index]}");
        if (!string.IsNullOrEmpty(stageNames[index]))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(stageNames[index]);
        }
        else
        {
            Debug.LogError($"Stage name at index {index} is null or empty!");
        }
    }
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll(); // Clear all saved progress
        PlayerPrefs.Save();
        Debug.Log("All progress has been reset.");
        UpdateButtonStates(); // Update button states after resetting
    }
}