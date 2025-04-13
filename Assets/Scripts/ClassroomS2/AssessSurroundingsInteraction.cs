using UnityEngine;
using UnityEngine.UI;

public class AssessSurroundingsInteraction : MonoBehaviour
{
    public GameObject[] dangerousObjects; // Array of objects to mark as dangerous
    public GameObject xMarkPrefab; // Prefab for the "X" mark to indicate danger
    public Button xMarkButton; // Single button for enabling marking mode and marking objects
    private bool[] isObjectMarked; // Tracks if each object is marked
    private bool isTaskCompleted = false; // Tracks if the task is completed
    private bool isMarkingModeActive = false; // Tracks if marking mode is active
    private GameObject currentNearbyObject; // Tracks the object the player is near

    private void Start()
    {
        // Initialize the marked status for all objects
        isObjectMarked = new bool[dangerousObjects.Length];

        // Debug log to check the dangerousObjects array
        foreach (var obj in dangerousObjects)
        {
            Debug.Log($"Dangerous Object: {obj.name}, Tag: {obj.tag}, Position: {obj.transform.position}");
        }

        // Ensure the button is set up
        if (xMarkButton != null)
        {
            xMarkButton.onClick.AddListener(OnXMarkButtonPressed); // Add listener to the button
            xMarkButton.gameObject.SetActive(false); // Hide the button at the start
        }
        else
        {
            Debug.LogError("X Mark Button is not assigned in the Inspector!");
        }
    }

    private void OnXMarkButtonPressed()
    {
        Debug.Log("X Mark Button Pressed");

        if (isTaskCompleted)
        {
            Debug.Log("Task already completed. No further marking is allowed.");
            return;
        }

        if (!isMarkingModeActive)
        {
            isMarkingModeActive = true;
            Debug.Log("Marking mode enabled. Go near objects and press the button to mark them.");
        }
        else
        {
            MarkNearbyObject();
        }
    }

    public void MarkNearbyObject()
    {
        if (currentNearbyObject == null)
        {
            Debug.Log("No nearby object to mark.");
            return;
        }

        for (int i = 0; i < dangerousObjects.Length; i++)
        {
            if (dangerousObjects[i] == currentNearbyObject && !isObjectMarked[i])
            {
                Debug.Log($"Player is near {currentNearbyObject.name}. Marking it as dangerous.");
                isObjectMarked[i] = true;
                PlaceXMark(currentNearbyObject);

                Debug.Log($"Marked {currentNearbyObject.name} as dangerous.");

                if (AreAllObjectsMarked())
                {
                    CompleteAssessment();
                }

                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DangerousObject"))
        {
            Debug.Log($"Player entered the vicinity of {other.name}.");
            currentNearbyObject = other.gameObject;

            if (xMarkButton != null && !isTaskCompleted)
            {
                xMarkButton.gameObject.SetActive(true); // Show the button
                Debug.Log("X Mark Button is now visible.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DangerousObject") && currentNearbyObject == other.gameObject)
        {
            Debug.Log($"Player exited the vicinity of {other.name}.");
            currentNearbyObject = null;

            // Add a delay before hiding the button to prevent premature hiding
            if (xMarkButton != null)
            {
                Invoke(nameof(HideXMarkButton), 0.2f); // Adjust the delay as needed
            }
        }
    }

    private void HideXMarkButton()
    {
        // Ensure the player is not near any object before hiding the button
        if (currentNearbyObject == null && xMarkButton != null)
        {
            xMarkButton.gameObject.SetActive(false);
            Debug.Log("X Mark Button is now hidden.");
        }
    }

    private void PlaceXMark(GameObject obj)
    {
        if (xMarkPrefab != null)
        {
            Vector3 xMarkPosition = obj.transform.position + Vector3.up * 0.9f + Vector3.forward * .2f; // Adjust height as needed
            Instantiate(xMarkPrefab, xMarkPosition, Quaternion.identity);
            Debug.Log($"Placed X mark on {obj.name}.");
        }
        else
        {
            Debug.LogError("xMarkPrefab is not assigned!");
        }
    }

    private bool AreAllObjectsMarked()
    {
        // Check if all objects are marked
        foreach (bool marked in isObjectMarked)
        {
            if (!marked)
            {
                return false;
            }
        }
        return true;
    }

    private void CompleteAssessment()
    {
        if (isTaskCompleted)
        {
            return;
        }

        // Mark the task as completed
        isTaskCompleted = true;
        isMarkingModeActive = false; // Disable marking mode

        // Hide the button
        if (xMarkButton != null)
        {
            xMarkButton.gameObject.SetActive(false);
        }

        // Notify the quest manager
        FindObjectOfType<QuestClipboardManagerS5>()?.CompleteTask(1); // Task index 1
        Debug.Log("All dangerous objects have been marked. Task completed.");
    }
}