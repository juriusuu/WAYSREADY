using UnityEngine;
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
}