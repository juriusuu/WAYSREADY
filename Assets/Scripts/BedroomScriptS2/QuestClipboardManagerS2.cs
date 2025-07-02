
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.UI;

public class QuestClipboardManagerS2 : MonoBehaviour
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
}