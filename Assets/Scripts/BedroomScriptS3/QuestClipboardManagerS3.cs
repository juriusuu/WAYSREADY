
using UnityEngine;
using UnityEngine.UI;

public class QuestClipboardManagerS3 : MonoBehaviour
{
    public GameObject clipboardPanel; // Reference to the clipboard panel
    public GameObject helpButton; // Reference to the help button
    public Toggle[] taskCheckboxes; // Array of checkboxes for tasks

    private bool[] taskCompletionStatus; // Tracks the completion status of tasks

    public Button proceedButton; // Reference to the proceed button

    private void Start()
    {
        // Ensure the clipboard panel is hidden at the start
        if (clipboardPanel != null)
        {
            clipboardPanel.SetActive(false);
        }

        // Initialize task completion status
        taskCompletionStatus = new bool[taskCheckboxes.Length];

        // Ensure all checkboxes are unchecked at the start
        UpdateCheckboxes();

        // Disable the proceed button at the start
        if (proceedButton != null)
        {
            proceedButton.interactable = false;
        }


        // Add a listener to the help button if it exists
        if (helpButton != null)
        {
            helpButton.GetComponent<Button>().onClick.AddListener(ToggleClipboard);
        }

        /*      // Add a listener to the proceed button
             if (proceedButton != null)
             {
                 proceedButton.onClick.AddListener(() =>
                 {
                     Debug.Log("Proceed button clicked. Transitioning to the next scene...");
                     GameManager.Instance.TransitionToNextState(); // Call the GameManager to transition to the next state
                 });
             } */
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
        }

        // Update the checkboxes
        UpdateCheckboxes();

        // Check if all tasks are completed
        if (AreAllTasksCompleted())
        {
            Debug.Log("All tasks are completed. Calling RewardPlayer...");
            if (proceedButton != null)
            {
                proceedButton.interactable = true; // Enable the button
            }
            RewardPlayer();
        }
        else
        {
            Debug.Log("Not all tasks are completed yet.");
        }
    }

    private void UpdateCheckboxes()
    {
        // Update the checkboxes to reflect task completion
        for (int i = 0; i < taskCheckboxes.Length; i++)
        {
            if (taskCheckboxes[i] != null)
            {
                taskCheckboxes[i].isOn = taskCompletionStatus[i];
            }
        }
    }

    private bool AreAllTasksCompleted()
    {
        foreach (bool isCompleted in taskCompletionStatus)
        {
            if (!isCompleted)
            {
                Debug.Log("Not all tasks are completed.");
                return false; // If any task is not completed, return false
            }
        }
        Debug.Log("All tasks are completed.");
        return true; // All tasks are completed
    }
    private void RewardPlayer()
    {
        Debug.Log("All tasks completed! Rewarding the player with coins.");

        if (GameSaveManager.Instance != null)
        {
            Debug.Log("Rewarding the player and saving the game...");
            GameSaveManager.Instance.RewardAndSave(50); // Reward 50 coins and save the game
        }
        else
        {
            Debug.LogError("GameSaveManager instance is null! Unable to reward and save the game.");
        }
    }

    /*  private void RewardPlayer()
     {
         Debug.Log("All tasks completed! Rewarding the player with coins.");

         // Reward coins and save the game using GameSaveManager
         GameSaveManager gameSaveManager = FindObjectOfType<GameSaveManager>();
         if (gameSaveManager != null)
         {
             Debug.Log("Rewarding the player and saving the game...");
             gameSaveManager.RewardAndSave(50); // Reward 50 coins and save the game
         }
         else
         {
             Debug.LogError("GameSaveManager instance is null! Unable to reward and save the game.");
         }
     } */
}
/* public class QuestClipboardManager : MonoBehaviour
{
    public GameObject clipboardPanel; // Reference to the clipboard panel
    public GameObject helpButton; // Reference to the help button
    public Toggle[] taskCheckboxes; // Array of checkboxes for tasks

    private bool[] taskCompletionStatus; // Tracks the completion status of tasks

    private void Start()
    {
        // Ensure the clipboard panel is hidden at the start
        if (clipboardPanel != null)
        {
            clipboardPanel.SetActive(false);
        }

        // Initialize task completion status
        taskCompletionStatus = new bool[taskCheckboxes.Length];

        // Ensure all checkboxes are unchecked at the start
        UpdateCheckboxes();

        // Add a listener to the help button if it exists
        if (helpButton != null)
        {
            helpButton.GetComponent<Button>().onClick.AddListener(ToggleClipboard);
        }
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
        }

        // Update the checkboxes
        UpdateCheckboxes();

        // Check if all tasks are completed
        if (AreAllTasksCompleted())
        {
            Debug.Log("All tasks are completed. Calling RewardPlayer...");
            RewardPlayer();
        }
        else
        {
            Debug.Log("Not all tasks are completed yet.");
        }
    }

    private void UpdateCheckboxes()
    {
        // Update the checkboxes to reflect task completion
        for (int i = 0; i < taskCheckboxes.Length; i++)
        {
            if (taskCheckboxes[i] != null)
            {
                taskCheckboxes[i].isOn = taskCompletionStatus[i];
            }
        }
    }

    private bool AreAllTasksCompleted()
    {
        foreach (bool isCompleted in taskCompletionStatus)
        {
            if (!isCompleted)
            {
                Debug.Log("Not all tasks are completed.");
                return false; // If any task is not completed, return false
            }
        }
        Debug.Log("All tasks are completed.");
        return true; // All tasks are completed
    }

    private void RewardPlayer()
    {
        Debug.Log("All tasks completed! Rewarding the player with coins.");

        // Reward coins for completing all tasks
        if (InventoryManagers.Instance != null)
        {
            InventoryManagers.Instance.AddCoins(50); // Add 50 coins to the player's inventory
        }
        else
        {
            Debug.LogError("InventoryManagers.Instance is null! Coins cannot be added.");
        }

        // Save the game state using GameSaveManager
         GameSaveManager gameSaveManager = FindObjectOfType<GameSaveManager>();
        if (gameSaveManager != null)
        {
            Debug.Log("Saving the game after completing all tasks...");
            InventoryManager.SaveGame();
        }
        else
        {
            Debug.LogError("GameSaveManager instance is null! Unable to save the game.");
        } 
    }
} */

/* using UnityEngine;
using UnityEngine.UI;

public class QuestClipboardManager : MonoBehaviour
{
    public GameObject clipboardPanel; // Reference to the clipboard panel
    public GameObject helpButton; // Reference to the help button
    public Toggle[] taskCheckboxes; // Array of checkboxes for tasks

    private bool[] taskCompletionStatus; // Tracks the completion status of tasks

    private void Start()
    {
        // Ensure the clipboard panel is hidden at the start
        if (clipboardPanel != null)
        {
            clipboardPanel.SetActive(false);
        }

        // Initialize task completion status
        taskCompletionStatus = new bool[taskCheckboxes.Length];

        // Ensure all checkboxes are unchecked at the start
        UpdateCheckboxes();
        

        // Add a listener to the help button if it exists
        if (helpButton != null)
        {
            helpButton.GetComponent<Button>().onClick.AddListener(ToggleClipboard);
        }
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
        }

        // Update the checkboxes
        UpdateCheckboxes();
    }

    private void UpdateCheckboxes()
    {
        // Update the checkboxes to reflect task completion
        for (int i = 0; i < taskCheckboxes.Length; i++)
        {
            if (taskCheckboxes[i] != null)
            {
                taskCheckboxes[i].isOn = taskCompletionStatus[i];
            }
        }
    }
} */