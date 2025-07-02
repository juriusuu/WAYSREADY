/* using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TaymerManager : MonoBehaviour
{
    public Image timerImage; // Reference to the TimerImage
    private float remainingTime; // Remaining time during gameplay

    public LayfManager layfManager; // Reference to the LifeManager (handles lives and hearts)
    public GameObject failPanel; // Reference to the fail panel UI

    private int hintsUsed = 0; // Counter for used hints
    public List<GameObject> objectsToHighlight; // List of objects to highlight
    public GameObject arrowPrefab; // Assign the arrow prefab in the Inspector
    private GameObject currentArrow; // Store the current arrow instance

    private bool isPlayerDead = false; // Flag to prevent repeated calls to HandlePlayerDeath

    private void Start()
    {
        // Access StageDataSO from GameManager
        if (GameManager.Instance != null && GameManager.Instance.currentStageData != null)
        {
            StageDataSO stageData = GameManager.Instance.currentStageData;
            remainingTime = stageData.totalTime; // Initialize time from StageDataSO
            hintsUsed = 0; // Reset hints used
        }
        else
        {
            Debug.LogError("StageDataSO is not assigned in GameManager!");
            remainingTime = 60f; // Fallback to default time
        }

        timerImage.fillAmount = 1f; // Initialize the timer UI

        if (failPanel != null)
        {
            failPanel.SetActive(false); // Ensure the fail panel is hidden at the start
        }
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime; // Decrease the remaining time
            timerImage.fillAmount = remainingTime / GameManager.Instance.currentStageData.totalTime; // Update the fill amount
        }
        else if (!isPlayerDead) // Only call HandlePlayerDeath once
        {
            HandlePlayerDeath(); // Call the method to handle player death when time runs out
            isPlayerDead = true; // Set the flag to true to prevent repeated calls
        }
    }

    public void UseHint()
    {
        if (hintsUsed >= GameManager.Instance.currentStageData.maxHints)
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
            Debug.Log($"Hint used: {hintsUsed}/{GameManager.Instance.currentStageData.maxHints}");
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

    public void ResetTimer()
    {
        remainingTime = GameManager.Instance.currentStageData.totalTime; // Reset the timer
        timerImage.fillAmount = 1f; // Reset the timer UI
        Debug.Log("Timer reset.");
    }

    private void HandlePlayerDeath()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.PlayerDead); // Notify the GameManager to handle player death
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    public void AddTime(float seconds)
    {
        remainingTime += seconds;
        if (remainingTime > GameManager.Instance.currentStageData.totalTime)
        {
            remainingTime = GameManager.Instance.currentStageData.totalTime; // Cap the time at the maximum
        }
        timerImage.fillAmount = remainingTime / GameManager.Instance.currentStageData.totalTime; // Update the timer UI
        Debug.Log($"{seconds} seconds added. Remaining time: {remainingTime}");
    }

    private void RestartScene()
    {
        remainingTime = GameManager.Instance.currentStageData.totalTime; // Reset the timer
        timerImage.fillAmount = 1f; // Reset the timer UI
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    private void ShowFailPanel()
    {
        if (failPanel != null)
        {
            failPanel.SetActive(true); // Show the fail panel
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Debug.LogError("Fail panel is not assigned!");
        }
    }
} */

//Without using SO
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TaymerManager : MonoBehaviour
{
    public Image timerImage; // Reference to the TimerImage
    private float totalTime; // Total time in seconds
    public float remainingTime;
    private GameObject currentHintTarget = null; // Add this at the top with your other fields

    public LayfManager layfManager; // Reference to the LifeManager (handles lives and hearts)
    public GameObject failPanel; // Reference to the fail panel UI

    public int totalHintsAllowed; // Total number of hints allowed per stage
    private int hintsUsed = 0; // Counter for used hints

    public List<GameObject> objectsToHighlight; // List of objects to highlight
    public GameObject arrowPrefab; // Assign the arrow prefab in the Inspector
    private GameObject currentArrow; // Store the current arrow instance

    public float timePenaltyPerHint = 30f; // Set this in the Inspector or change the value
    public float currentTime; // Your timer variable

    public float GetCurrentTime()
    {
        return currentTime;
    }


    /*   private void Start()
      {
          remainingTime = totalTime; // Initialize the remaining time
          timerImage.fillAmount = 1f; // Start with a full bar

          if (failPanel != null)
          {
              failPanel.SetActive(false); // Ensure the fail panel is hidden at the start
          }
      } */

    private void Start()
    {
        // Get the current scene name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Fetch the default time for the current scene from GameManager
        if (GameManager.Instance != null)
        {
            totalTime = GameManager.Instance.GetDefaultTimeForScene(currentSceneName);
            remainingTime = totalTime; // Initialize remaining time
            Debug.Log($"Default time for scene '{currentSceneName}': {totalTime} seconds");
            Debug.Log($"BOBOTIME '{GameManager.Instance.additionalTime}");
            Debug.Log($"BOBOHINT'{GameManager.Instance.additionalHints}");
            Debug.Log($"BOBOLAYF'{GameManager.Instance.additionalLives}");


            // Add additional time purchased from the shop
            if (GameManager.Instance.additionalTime > 0)
            {
                Debug.Log($"GameManager additional time before addition: {GameManager.Instance.additionalTime}");

                // Add the additional time to both remainingTime and totalTime
                totalTime += GameManager.Instance.additionalTime;
                remainingTime = totalTime; // Update remaining time to reflect the new total

                Debug.Log($"Remaining time after addition: {remainingTime}, Total time after addition: {totalTime}");

                // Reset the additional time after applying
                GameManager.Instance.additionalTime = 0;
                Debug.Log($"Default Time: {totalTime}, Additional Time: {GameManager.Instance.additionalTime}");

            }
            else
            {
                remainingTime = totalTime; // Initialize remaining time
            }
        }
        else
        {
            Debug.LogError("GameManager instance is null! Using fallback default time.");
            totalTime = 60f; // Fallback default time
            remainingTime = totalTime;
        }


        // Initialize the timer UI
        timerImage.fillAmount = 1f;
        /*    // Add additional lives purchased from the shop
           if (GameManager.Instance.additionalLives > 0)
           {
               if (layfManager != null)
               {
                   layfManager.AddLives(GameManager.Instance.additionalLives); // Add lives to LayfManager
                   Debug.Log($"Additional lives applied: {GameManager.Instance.additionalLives}");
                   GameManager.Instance.additionalLives = 0; // Reset additional lives after applying
               }
               else
               {
                   Debug.LogError("LayfManager is not assigned! Unable to add additional lives.");
               }
           } */


        // Fetch the default hints for the current scene from GameManager
        if (GameManager.Instance != null)
        {
            totalHintsAllowed = GameManager.Instance.GetDefaultHintsForScene(currentSceneName);
            Debug.Log($"Default hints for scene '{currentSceneName}': {totalHintsAllowed}");

            // Apply additional hints purchased from the shop
            if (GameManager.Instance.additionalHints > 0)
            {
                totalHintsAllowed += GameManager.Instance.additionalHints;
                Debug.Log($"Additional hints applied: {GameManager.Instance.additionalHints}. Total hints allowed: {totalHintsAllowed}");

                // Reset the additional hints after applying
                GameManager.Instance.additionalHints = 0;
            }
        }
        else
        {
            Debug.LogError("GameManager instance is null! Using fallback default hints.");
            totalHintsAllowed = 0; // Fallback default hints
        }

        // Initialize the hintsUsed counter
        hintsUsed = 0;



        // Ensure the fail panel is hidden at the start
        if (failPanel != null)
        {
            failPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Fail panel is not assigned! Please assign it in the Inspector.");
        }
        isPlayerDead = false; // Ensure the death flag is reset
        // Start the timer coroutine
        StartCoroutine(TimerCoroutine());
    }
    private int GetTaskIndexForTarget(GameObject target)
    {
        return objectsToHighlight.IndexOf(target);
    }
    public void AddAdditionalTime(float timeToAdd)
    {
        remainingTime += timeToAdd;
        totalTime += timeToAdd; // Update the total time dynamically
        timerImage.fillAmount = remainingTime / totalTime; // Update the timer UI
        Debug.Log($"{timeToAdd} seconds added. Remaining time: {remainingTime}, Total time: {totalTime}");
    }
    private System.Collections.IEnumerator TimerCoroutine()
    {
        Debug.Log("[TaymerManager] TimerCoroutine started.");

        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            remainingTime -= 1f; // Decrease time by 1 second
            // Update the timer UI
            timerImage.fillAmount = remainingTime / totalTime;
        }

        // Handle player death when time runs out
        if (!isPlayerDead)
        {
            Debug.Log("[TaymerManager] Timer ran out in TimerCoroutine. Calling HandlePlayerDeath.");
            HandlePlayerDeath();
            isPlayerDead = true;
        }
    }
    public bool isPlayerDead = false; // Flag to prevent repeated calls to HandlePlayerDeath
    private void Update()
    {
        /*         Debug.Log("[TaymerManager] Update is running."); // Add this log to confirm Update is being called
         */
        /*     if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime; // Decrease the remaining time
                                                 // timerImage.fillAmount = remainingTime / totalTime; // Update the fill amount
                                                 //  Debug.Log($"[TaymerManager] Timer running. Remaining time: {remainingTime}");
            }
            else if (!isPlayerDead) // Only call HandlePlayerDeath once per death
            {
                Debug.Log("[TaymerManager] Timer ran out. Calling HandlePlayerDeath.");
                isPlayerDead = true; // Set the flag to true to prevent repeated calls
                NotifyGameManager(); // Call the method to handle player death when time runs out

            } */

        /*   // Remove arrow if the target object is gone or inactive
          if (currentArrow != null && (currentHintTarget == null || !currentHintTarget.activeInHierarchy))
          {
              Destroy(currentArrow);
              currentArrow = null;
              currentHintTarget = null;
          }
   */

        // Add this to your Update() method (inside Update, not as a new method):
        /*    if (currentArrow != null && currentHintTarget != null)
           {
               GameObject player = GameObject.FindWithTag("Player");
               if (player != null)
               {
                   // Update arrow position above player
                   currentArrow.transform.position = player.transform.position + Vector3.up * 1.5f;

                   // Update arrow rotation to point to the target
                   Vector3 direction = (currentHintTarget.transform.position - player.transform.position).normalized;
                   Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
                   currentArrow.transform.rotation = lookRotation;
               }
           } */
        /*     if (currentArrow != null && currentHintTarget != null)
            {
                UpdateArrowDirection();
                // Keep the arrow above the player
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                    currentArrow.transform.position = player.transform.position + Vector3.up * 2f;
            } */


        // Update arrow direction and position when player moves
        if (currentArrow != null && currentHintTarget != null)
        {
            UpdateArrowDirection();
            // Keep the arrow above the player
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                currentArrow.transform.position = player.transform.position + Vector3.up * 1.2f;
        }

        if (currentArrow != null && (
        currentHintTarget == null ||
        !currentHintTarget.activeInHierarchy ||
        IsQuestTaskDone(currentHintTarget) // <-- Add this check
    ))
        {
            Destroy(currentArrow);
            currentArrow = null;
            currentHintTarget = null;
        }
        currentTime = remainingTime;
    }
    /*    private void Update()
       {
           if (remainingTime > 0)
           {
               remainingTime -= Time.deltaTime;
           }
           else if (!isPlayerDead)
           {
               isPlayerDead = true;
               NotifyGameManager(); // Notify GameManager when timer reaches 0
           }
       } */
    // Add this method to your class:
    private bool IsQuestTaskDone(GameObject target)
    {
        var questManager = FindFirstObjectByType<QuestClipboardManager>();
        if (questManager != null)
        {
            int taskIndex = GetTaskIndexForTarget(target);
            if (taskIndex >= 0)
                return questManager.IsTaskDone(taskIndex);
        }
        return false;
    }
    private void NotifyGameManager()
    {
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            Debug.Log("[TaymerManager] Timer expired. Notifying GameManager to handle player death.");
            HandlePlayerDeath(); // Call HandlePlayerDeath in GameManager
        }
        else
        {
            Debug.LogError("[TaymerManager] GameManager not found! Unable to notify.");
        }
    }
    /*     private void Update()
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime; // Decrease the remaining time
                timerImage.fillAmount = remainingTime / totalTime; // Update the fill amount
                Debug.Log($"Remaining Time: {remainingTime}, Fill Amount: {timerImage.fillAmount}");

            }
            else if (!isPlayerDead) // Only call HandlePlayerDeath once
            {
                HandlePlayerDeath(); // Call the method to handle player death when time runs out
                isPlayerDead = true; // Set the flag to true to prevent repeated calls
            }
        } */
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

            if (IsQuestTaskDone(nearestObject))
            {
                Debug.Log("Task for this object is already completed. No hint needed.");
                return;
            }

            AttachArrowToObject(nearestObject);
            Debug.Log($"Item Hint: {nearestObject}");

            // Only remove and increment if hint was successfully applied
            objectsToHighlight.Remove(nearestObject);
            hintsUsed++;
            Debug.Log($"Hint used: {hintsUsed}/{totalHintsAllowed}");

            // Decrease the timer as a penalty for using a hint
            remainingTime -= timePenaltyPerHint;
            if (remainingTime < 0) remainingTime = 0; // Prevent negative time

            Debug.Log($"Hint used: {hintsUsed}/{totalHintsAllowed}. Time penalty applied: {timePenaltyPerHint} seconds.");
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
    }/* 

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
 *//* 
    private void AttachArrowToObject(GameObject targetItem)
    {
        // Remove the previous arrow if it exists
        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }

        // Find the player
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found!");
            return;
        }

        // Position the arrow above the player
        Vector3 arrowPosition = player.transform.position + Vector3.up * 1.5f;

        // Calculate direction from player to target item
        Vector3 direction = (targetItem.transform.position - player.transform.position).normalized;

        // Set the arrow's rotation to point toward the item
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Instantiate the arrow
        currentArrow = Instantiate(arrowPrefab, arrowPosition, lookRotation);

        // Optionally, scale the arrow
        currentArrow.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);

        // Make the arrow a child of the player so it moves with them
        currentArrow.transform.SetParent(player.transform);

        // Store the current target for real-time updating
        currentHintTarget = targetItem;

        Debug.Log($"Arrow placed near player, pointing to: {targetItem.name}");
    }
 */
    private void AttachArrowToObject(GameObject targetItem)
    {
        // Remove the previous arrow if it exists
        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }

        // Find the player
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found!");
            return;
        }

        // Position the arrow above the player
        /*   Vector3 arrowPosition = player.transform.position + Vector3.up * 0.1f; // Adjust height as needed
   */
        /*   float arrowHeight = 1f; // Replace with your arrow's height in model units
          float scale = 15f;
          float offset = arrowHeight * scale / 2f; // If pivot is at base
          Vector3 arrowPosition = player.transform.position + Vector3.up * (0.1f + offset); */


        // Place the arrow at a fixed height above the player, regardless of scale
        float desiredHeightAboveHead = 0.5f; // Adjust as needed
        Vector3 arrowPosition = player.transform.position + Vector3.up * desiredHeightAboveHead;

        // Instantiate the arrow
        currentArrow = Instantiate(arrowPrefab, arrowPosition, Quaternion.identity);

        // --- Add this block to offset the arrow downward based on its scale and model height ---
        float arrowModelHeight = 1f; // Replace with your arrow's Y size in model units (check in Mesh Renderer bounds)
        float downwardOffset = arrowModelHeight * currentArrow.transform.localScale.y / 2f;
        currentArrow.transform.position -= Vector3.up * downwardOffset;

        // Make the arrow a child of the player so it moves with them
        currentArrow.transform.SetParent(player.transform);
        currentArrow.transform.localPosition = new Vector3(0, 1.2f, 0); // Adjust as needed
        // Set the scale to make it visible
        currentArrow.transform.localScale = new Vector3(30f, 10f, 10f); // Adjust as needed

        // Store the current target for real-time updating
        currentHintTarget = targetItem;

        // Set the initial rotation to point at the target
        UpdateArrowDirection();
    }

    // Add this method if you don't have it yet:
    private void UpdateArrowDirection()
    {
        if (currentArrow == null || currentHintTarget == null) return;

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        // Calculate direction from player to target
        Vector3 direction = (currentHintTarget.transform.position - player.transform.position).normalized;

        // Set the arrow's rotation to point toward the target
        if (direction != Vector3.zero)
            /*   currentArrow.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    */
            currentArrow.transform.rotation = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.Euler(0, 90, 0);
    }
    public void StartTimer()
    {
        if (remainingTime > 0)
        {
            StopAllCoroutines(); // Stop any existing timer coroutine
            StartCoroutine(TimerCoroutine()); // Restart the timer coroutine
            Debug.Log("Timer started.");
        }
        else
        {
            Debug.LogWarning("Timer cannot be started because remaining time is 0.");
        }
    }

    //settimer
    public void SetTime(float time)
    {
        remainingTime = time;
        currentTime = time;
        if (totalTime > 0 && timerImage != null)
            timerImage.fillAmount = remainingTime / totalTime;
    }


    /*     public void ResetTimer()
        {
            remainingTime = totalTime; // Reset the timer
            timerImage.fillAmount = 1f; // Reset the timer UI
            Debug.Log("Timer reset.");

            // Restart the timer coroutine
            StopAllCoroutines(); // Stop any existing timer coroutine
            StartCoroutine(TimerCoroutine()); // Start a new timer coroutine
            Debug.Log("Timer coroutine restarted.");
        } */

    public void ResetTimer()
    {
        // Reset the remaining time to the total time
        remainingTime = totalTime;

        // Update the timer UI to show a full timer
        timerImage.fillAmount = 1f;
        isPlayerDead = false; // Reset the death flag
        Debug.Log("[TaymerManager] Timer reset. Remaining time set to total time.");

        // Log the reset for debugging
        Debug.Log("Timer reset. Remaining time set to total time.");

        // Stop any existing timer coroutine to avoid multiple coroutines running
        StopAllCoroutines();

        // Restart the timer coroutine
        StartCoroutine(TimerCoroutine());
        Debug.Log("Timer coroutine restarted.");
    }


    private void HandlePlayerDeath()
    {
        {
            Debug.Log("HandlePlayerDeath called. Notifying GameManager.");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeState(GameManager.GameState.PlayerDead); // Notify the GameManager to handle player death
            }
            else
            {
                Debug.LogError("GameManager instance not found!");
            }
        }
    }

    /*             if (layfManager != null)
                {
                    layfManager.LoseLife();
                    if (layfManager.GetRemainingLives() > 0)
                    {
                        Debug.Log("Player lost a life. Restarting the scene...");
                        RestartScene();
                    }
                    else
                    {
                        Debug.Log("No lives remaining. Showing fail panel...");
                        ShowFailPanel();
                    }
                }
                else
                {
                    Debug.LogError("LayfManager is not assigned!");
                }

            } */


    /*     public void AddTime(float seconds)
        {
            remainingTime += seconds;
            if (remainingTime > totalTime)
            {
                remainingTime = totalTime; // Cap the time at the maximum
            }
            // timerImage.fillAmount = remainingTime / totalTime; // Update the timer UI
            Debug.Log($"{seconds} seconds added. Remaining time: {remainingTime}");
        } */

    public void AddTime(float seconds)
    {
        remainingTime += seconds;
        totalTime += seconds; // Update the total time dynamically
        timerImage.fillAmount = remainingTime / totalTime; // Update the timer UI
        Debug.Log($"{seconds} seconds added. Remaining time: {remainingTime}, Total time: {totalTime}");
    }


    public void AddHint()
    {
        totalHintsAllowed++; // Increment the total hints allowed
        Debug.Log($"Hint added. Total hints allowed: {totalHintsAllowed}");
    }

    private void RestartScene()
    {
        remainingTime = totalTime; // Reset the timer
        timerImage.fillAmount = 1f; // Reset the timer UI
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    private void ShowFailPanel()
    {
        if (failPanel != null)
        {
            failPanel.SetActive(true); // Show the fail panel
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Debug.LogError("Fail panel is not assigned!");
        }
    }
}

///////////

/*         if (layfManager != null)
                 {
                     layfManager.LoseLife(); // Decrease the player's life

                     if (layfManager.GetRemainingLives() > 0) // Check if the player has lives left
                     {
                         Debug.Log("Player lost a life. Restarting the scene...");
                         GameManager.Instance.ChangeState(GameManager.GameState.PlayerDead); // Notify the GameManager
                     }
                     else
                     {
                         Debug.Log("No lives remaining. Transitioning to GameOver state...");
                         GameManager.Instance.ChangeState(GameManager.GameState.GameOver); // Notify the GameManager
                     }
                 }
                 else
                 {
                     Debug.LogError("LifeManager is not assigned!");
                 } */

/* using UnityEngine;
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
    } */

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


/*   // Add additional time purchased from the shop
  if (GameManager.Instance.additionalTime > 0)
  {
      Debug.Log($"GameManager additional time before addition: {GameManager.Instance.additionalTime}");

      // Add the additional time to both remainingTime and totalTime
      totalTime += GameManager.Instance.additionalTime;

      // Debug.Log($"Remaining time after addition: {remainingTime}, Total time after addition: {totalTime}");

      // Reset the additional time after applying
      GameManager.Instance.additionalTime = 0;
  }
  remainingTime = totalTime; // Update remaining time to reflect the new total
  Debug.Log($"Remaining time after addition: {remainingTime}, Total time after addition: {totalTime}");
*/