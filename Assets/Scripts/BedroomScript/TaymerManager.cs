using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TaymerManager : MonoBehaviour
{
    public Image timerImage; // Reference to the TimerImage
    public float totalTime = 60f; // Total time in seconds
    private float remainingTime;

    public LayfManager layfManager; // Reference to the LifeManager (handles lives and hearts)

    public int totalHintsAllowed = 3; // Total number of hints allowed per stage
    private int hintsUsed = 0; // Counter for used hints

    public List<GameObject> objectsToHighlight; // List of objects to highlight
    public GameObject arrowPrefab; // Assign the arrow prefab in the Inspector
    private GameObject currentArrow; // Store the current arrow instance

    private void Start()
    {
        remainingTime = totalTime; // Initialize the remaining time
        timerImage.fillAmount = 1f; // Start with a full bar
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime; // Decrease the remaining time
            timerImage.fillAmount = remainingTime / totalTime; // Update the fill amount
        }
        else
        {
            HandlePlayerDeath(); // Call the method to handle player death when time runs out
        }
    }

    public void UseHint()
    {
        if (hintsUsed >= totalHintsAllowed)
        {
            Debug.Log("No more hints available for this stage.");
            return; // Exit if the player has used all hints
        }

        GameObject nearestObject = GetNearestObject(); // Find the nearest object
        if (nearestObject != null)
        {
            AttachArrowToObject(nearestObject); // Attach the arrow to the nearest object
            objectsToHighlight.Remove(nearestObject); // Remove it from the list
            hintsUsed++; // Increment the hint counter
            Debug.Log($"Hint used: {hintsUsed}/{totalHintsAllowed}");
        }
        else
        {
            Debug.Log("No more objects to highlight.");
        }
    }

    private GameObject GetNearestObject()
    {
        GameObject player = GameObject.FindWithTag("Player"); // Find the player
        if (player == null)
        {
            Debug.LogError("Player object not found!");
            return null;
        }

        GameObject nearestObject = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var obj in objectsToHighlight)
        {
            if (obj != null && obj.activeInHierarchy) // Check if the object is active
            {
                float distance = Vector3.Distance(player.transform.position, obj.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestObject = obj;
                }
            }
        }

        return nearestObject; // Return the nearest object
    }

    private void AttachArrowToObject(GameObject obj)
    {
        // Remove the previous arrow if it exists
        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }

        // Instantiate a new arrow close to the object
        Vector3 arrowPosition = obj.transform.position + Vector3.up * 0.2f; // Adjust height to 0.2 units above the object
        currentArrow = Instantiate(arrowPrefab, arrowPosition, Quaternion.Euler(45f, -90f, -90f)); // Set rotation

        // Scale the arrow to the desired size
        currentArrow.transform.localScale = new Vector3(0.07024757f, 0.07024757f, 0.07024757f);

        // Make the arrow a child of the object
        currentArrow.transform.SetParent(obj.transform);

        Debug.Log($"Arrow placed above object: {obj.name}");
    }
    private void HandlePlayerDeath()
    {
        if (layfManager != null)
        {
            layfManager.LoseLife(); // Decrease the player's life

            if (layfManager.GetRemainingLives() > 0) // Check if the player has lives left
            {
                Debug.Log("Player lost a life. Restarting the scene...");
                RestartScene(); // Restart the scene and reset the timer
            }
            else
            {
                Debug.Log("No lives remaining. Restarting the game...");
                RestartScene(); // Restart the scene if no lives are left
            }
        }
        else
        {
            Debug.LogError("LifeManager is not assigned!");
        }
    }

    private void RestartScene()
    {
        remainingTime = totalTime; // Reset the timer
        timerImage.fillAmount = 1f; // Reset the timer UI
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); // Reload the current scene
    }
    /*     private void HandlePlayerDeath()
        {
            if (layfManager != null)
            {
                layfManager.LoseLife(); // Decrease the player's life

                if (layfManager.GetRemainingLives() > 0) // Check if the player has lives left
                {
                    // Reset the timer for the next life
                    remainingTime = totalTime;
                    timerImage.fillAmount = 1f;
                    Debug.Log("Player lost a life. Timer reset.");
                }
                else
                {
                    Debug.Log("No lives remaining. Restarting the game...");
                    RestartGame(); // Restart the game if no lives are left
                }
            }
            else
            {
                Debug.LogError("LifeManager is not assigned!");
            }
        }

        private void RestartGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        } */
}