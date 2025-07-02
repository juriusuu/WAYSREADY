using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestClipboardManager : MonoBehaviour
{
    public string questName; // Unique name for this quest
    public GameObject clipboardPanel; // Reference to the clipboard panel
    public GameObject helpButton; // Reference to the help button
    public Toggle[] taskCheckboxes; // Array of checkboxes for tasks
    public Button proceedButton; // Reference to the proceed button
    public SceneLoaderButtonHelper sceneButtonHelper; // Reference to the SceneButtonHelper

    private bool[] taskCompletionStatus; // Tracks the completion status of tasks

    public static string panelToActivate; // Static variable to store the panel name

    private void Start()
    {
        // Ensure the clipboard panel is hidden at the start
        if (clipboardPanel != null)
        {
            clipboardPanel.SetActive(false);
        }

        // Initialize task completion status
        taskCompletionStatus = new bool[taskCheckboxes.Length];

        // Load saved state if it exists
        LoadState();

        // Ensure all checkboxes are updated
        UpdateCheckboxes();
        // Check if all tasks are completed and enable the proceed button
        if (AreAllTasksCompleted())
        {
            if (proceedButton != null)
            {
                proceedButton.interactable = true; // Enable the button
                proceedButton.onClick.AddListener(OnProceedButtonPressed); // Add listener for button press
                Debug.Log("Proceed button re-enabled on scene load.");
            }
            else
            {
                Debug.LogError("Proceed button is not assigned in the Inspector!");
            }
        }
        else
        {
            if (proceedButton != null)
            {
                proceedButton.interactable = false; // Keep the button disabled
                Debug.Log("Proceed button remains disabled on scene load.");
            }
        }
        // Add a listener to the help button if it exists
        if (helpButton != null)
        {
            helpButton.GetComponent<Button>().onClick.AddListener(ToggleClipboard);
        }
    }
    public bool IsTaskDone(int taskIndex)
    {
        if (taskIndex >= 0 && taskIndex < taskCompletionStatus.Length)
            return taskCompletionStatus[taskIndex];
        return false;
    }
    public void ToggleClipboard()
    {
        // Toggle the visibility of the clipboard panel
        if (clipboardPanel != null)
        {
            clipboardPanel.SetActive(!clipboardPanel.activeSelf);
        }

        // Update the checkboxes to reflect the current task completion status
        UpdateCheckboxes();
    }

    public void CompleteTask(int taskIndex)
    {
        // Mark the task as completed
        if (taskIndex >= 0 && taskIndex < taskCompletionStatus.Length)
        {
            taskCompletionStatus[taskIndex] = true;
            Debug.Log($"Task {taskIndex} marked as completed.");
        }

        // Update the checkboxes
        UpdateCheckboxes();

        if (AreAllTasksCompleted())
        {
            Debug.Log($"All tasks for quest '{questName}' are completed.");
            if (proceedButton != null)
            {
                proceedButton.interactable = true; // Enable the proceed button
                proceedButton.onClick.AddListener(OnProceedButtonPressed); // A
                Debug.Log("Proceed button is now interactable.");
            }
            else
            {
                Debug.LogError("Proceed button is not assigned in the Inspector!");
            }
        }
        else
        {
            Debug.Log("Not all tasks are completed yet.");
        }
        // Save the updated state
        SaveState();
    }

    private void UpdateCheckboxes()
    {
        // Update the checkboxes to reflect task completion
        for (int i = 0; i < taskCheckboxes.Length; i++)
        {
            if (taskCheckboxes[i] != null)
            {
                taskCheckboxes[i].isOn = taskCompletionStatus[i];
                Debug.Log($"Checkbox {i} updated to: {taskCompletionStatus[i]}");
            }
        }
    }

    /*    private bool AreAllTasksCompleted()
       {
           foreach (bool isCompleted in taskCompletionStatus)
           {
               if (!isCompleted)
               {
                   return false; // If any task is not completed, return false
               }
           }
           return true; // All tasks are completed
       } */
    private bool AreAllTasksCompleted()
    {
        for (int i = 0; i < taskCompletionStatus.Length; i++)
        {
            if (!taskCompletionStatus[i])
            {
                Debug.Log($"Task {i} is not completed.");
                return false;
            }
        }
        Debug.Log("All tasks are completed.");
        return true;
    }/* 
    private void OnProceedButtonPressed()
    {
        Debug.Log($"Proceed button pressed for quest '{questName}'.");

        // Reward the player
        RewardPlayer();

        // Use SceneButtonHelper to load the next scene
        if (sceneButtonHelper != null)
        {
            string nextSceneName = GetNextSceneName();
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                Debug.Log($"Using SceneButtonHelper to load next scene: {nextSceneName}");
                sceneButtonHelper.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogError("Next scene name is not set or invalid!");
            }
        }
        else
        {
            Debug.LogError("SceneButtonHelper is not assigned!");
        }
    }
 *//* 
    private void OnProceedButtonPressed()
    {
        Debug.Log($"Proceed button pressed for quest '{questName}'.");

        // Reward the player
        RewardPlayer();

        // Get the next scene or panel identifier
        string nextSceneOrPanel = GetNextSceneName();
        if (!string.IsNullOrEmpty(nextSceneOrPanel))
        {
            // Check if the next step is a storyline panel
            if (nextSceneOrPanel.Contains("Storyline"))
            {
                // Store the panel name in the static variable
                panelToActivate = nextSceneOrPanel;

                // Load the Main Menu scene
                Debug.Log($"Loading Main Menu scene to activate panel: {nextSceneOrPanel}");
                SceneManager.LoadScene("Main Menu"); // Replace "Main Menu" with the actual name of your Main Menu scene
            }
            else
            {
                // Otherwise, assume it's a scene and load it
                Debug.Log($"Loading scene: {nextSceneOrPanel}");
                SceneManager.LoadScene(nextSceneOrPanel);
            }
        }
        else
        {
            Debug.LogWarning("No next scene or panel defined.");
        }
    } */
    private void OnProceedButtonPressed()
    {
        Debug.Log($"Proceed button pressed for quest '{questName}'.");

        // Reward the player
        RewardPlayer();

        // Get the next scene or panel identifier
        string nextSceneOrPanel = GetNextSceneName();
        if (!string.IsNullOrEmpty(nextSceneOrPanel))
        {
            // Check if the next step is a storyline panel
            if (nextSceneOrPanel.Contains("Storyline"))
            {
                // Store the panel name in the static variable
                panelToActivate = nextSceneOrPanel;

                // Save the game before transitioning
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.SaveGame(SceneManager.GetActiveScene().name);
                    Debug.Log("Game saved before transitioning to the Main Menu.");
                }
                else
                {
                    Debug.LogError("GameManager instance is null! Unable to save the game.");
                }

                // Load the Main Menu scene
                Debug.Log($"Loading Main Menu scene to activate panel: {nextSceneOrPanel}");
                SceneManager.LoadScene("Main Menu"); // Replace "Main Menu" with the actual name of your Main Menu scene
            }
            else
            {
                // Otherwise, assume it's a scene and load it
                Debug.Log($"Loading scene: {nextSceneOrPanel}");

                // Save the game before transitioning
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.SaveGame(SceneManager.GetActiveScene().name);
                    Debug.Log("Game saved before transitioning to the next scene.");
                }
                else
                {
                    Debug.LogError("GameManager instance is null! Unable to save the game.");
                }

                SceneManager.LoadScene(nextSceneOrPanel);
            }
        }
        else
        {
            Debug.LogWarning("No next scene or panel defined.");
        }
    }
    private void RewardPlayer()
    {
        Debug.Log($"All tasks for quest '{questName}' completed! Rewarding the player with coins.");

        // Reward the player with coins
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddRewardCoins(50); // Reward 50 coins
            Debug.Log("Player rewarded with 50 coins.");
        }
        else
        {
            Debug.LogError("GameManager instance is null! Unable to reward the player.");
        }

        // Mark the current stage as completed
        if (GameManager.Instance != null)
        {
            GameManager.Instance.MarkSceneAsCompleted(questName); // Use the quest name as the stage identifier
            Debug.Log($"Stage '{questName}' marked as completed.");
        }
        else
        {
            Debug.LogError("GameManager instance is null! Unable to mark the stage as completed.");
        }
    }
    /* 
        // Helper method to determine the next scene name
        private string GetNextSceneName()
        {
            // Define the mapping of quest names to next scene names
            switch (questName)
            {
                case "Stage1Easy":
                    return "Stage1Normal";
                case "Stage1Normal":
                    return "Stage1Hard";
                case "Stage1Hard":
                    return "Stage2Easy";
                case "Stage2Easy":
                    return "Stage2Normal";
                case "Stage2Normal":
                    return "Stage2Hard";
                case "Stage2Hard":
                    return "Stage3Easy";
                case "Stage3Easy":
                    return "Stage3Normal";
                case "Stage3Normal":
                    return "Stage3Hard";
                case "Stage3Hard":
                    return null; // No next scene, final stage
                default:
                    return null; // No next scene
            }
        }
     */

    private string GetNextSceneName()
    {
        // Define the mapping of quest names to next scene names
        switch (questName)
        {
            case "Stage1Easy":
                return "Stage1Normal";
            case "Stage1Normal":
                return "Stage1Hard";
            case "Stage1Hard":
                return "Stage 1 Storyline  (20)"; // Transition to Storyline 2 after Stage1Hard
            case "Stage 2 Storyline":
                return "Stage2Easy";
            case "Stage2Easy":
                return "Stage2Normal";
            case "Stage2Normal":
                return "Stage2Hard";
            case "Stage2Hard":
                return "Stage 2 Storyline (17)"; // Transition to Storyline 3 after Stage2Hard
            case "Stage 3 Storyline":
                return "Stage3Easy";
            case "Stage3Easy":
                return "Stage3Normal";
            case "Stage3Normal":
                return "Stage3Hard";
            case "Stage3Hard":
                return "Stage 3 Storyline (20)"; // Transition to Congratulations panel after Stage3Hard
            default:
                return null; // No next scene
        }
    }
    public void SaveState()
    {
        // Save the task completion status to the GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveQuestState(questName, taskCompletionStatus);
            Debug.Log($"Saved task completion status for quest '{questName}': {string.Join(", ", taskCompletionStatus)}");

        }
        else
        {
            Debug.LogError("GameManager instance is null! Unable to save quest state.");
        }
    }

    public void LoadState()
    {
        if (string.IsNullOrEmpty(questName))
        {
            Debug.LogError("QuestClipboardManager: questName is null or empty. Cannot load state.");
            taskCompletionStatus = new bool[taskCheckboxes.Length]; // Initialize default state
            return;
        }

        // Load the task completion status from the GameManager
        if (GameManager.Instance != null)
        {
            taskCompletionStatus = GameManager.Instance.LoadQuestState(questName, taskCheckboxes.Length);
            Debug.Log($"Loaded task completion status for quest '{questName}': {string.Join(", ", taskCompletionStatus)}");

        }
        else
        {
            Debug.LogError("GameManager instance is null! Unable to load quest state.");
            taskCompletionStatus = new bool[taskCheckboxes.Length]; // Initialize default state
        }
    }
}