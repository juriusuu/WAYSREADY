using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaymerManagerHexafall : MonoBehaviour
{
    public Image timerImage; // Reference to the TimerImage
    public float totalTime = 60f; // Total time in seconds
    private float remainingTime;

    private PanelManager panelManager; // Reference to the PanelManager

    public GameObject player; // Reference to the player GameObject
    public float fallThreshold = -10f; // Y-axis threshold for detecting a fall

    private bool isGameOver = false; // Tracks if the game is already over
    public AdditionalPanelManager additionalPanelManager; // Reference to the AdditionalPanelManager

    // Add instruction system support (same as TaymerManagerSandbag)
    private bool gameStarted = false; // Track if the game has started
    private bool instructionsCompleted = false; // Track if instructions are done

    private void Start()
    {
        remainingTime = totalTime; // Initialize the remaining time
        timerImage.fillAmount = 1f; // Start with a full bar

        // Find the PanelManager in the scene
        panelManager = FindObjectOfType<PanelManager>();

        if (panelManager == null)
        {
            Debug.LogError("PanelManager not found in the scene!");
        }

        // DON'T start the timer yet - wait for instructions
        Debug.Log("Timer initialized but not started - waiting for instructions");
    }

    private void Update()
    {
        // Only run the timer and game logic if instructions are completed AND game has started
        if (instructionsCompleted && gameStarted && !isGameOver)
        {
            // Decrease the timer
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime; // Decrease the remaining time
                timerImage.fillAmount = remainingTime / totalTime; // Update the fill amount
            }
            else
            {
                HandleTimeOut(); // Call the method to handle timeout when time runs out
            }

            // Check if the player has fallen
            if (player != null && player.transform.position.y < fallThreshold)
            {
                HandlePlayerFall(); // Handle the player's fall
            }
        }
    }

    // Call this method from InstructionManager when instructions are done
    public void StartGame()
    {
        instructionsCompleted = true;
        gameStarted = true;
        Debug.Log("Game started! Timer is now running.");
    }

    private void HandleTimeOut()
    {
        Debug.Log("Time's up! Showing Finish Panel.");

        // Use the PanelManager to show the Finish Panel
        if (panelManager != null)
        {
            panelManager.ShowFinishPanel(); // Call the method to show the Finish Panel
        }
        else
        {
            Debug.LogError("PanelManager is not assigned or found!");
        }

        // Show the additional panel
        if (additionalPanelManager != null)
        {
            additionalPanelManager.ShowAdditionalPanel();
            StartCoroutine(WaitForAdditionalPanelToFinish(1f)); // Wait for the additional panel to finish
        }
        else
        {
            Debug.LogError("AdditionalPanelManager is not assigned!");
            MarkGameOver(); // If no additional panel, mark the game as over immediately
        }

        // Stop the timer from continuing to decrease
        remainingTime = 0;

        // Pause the game
        Time.timeScale = 0f;

        isGameOver = true; // Mark the game as over
    }

    private void HandlePlayerFall()
    {
        Debug.Log("Player has fallen! Showing Fail Panel.");

        // Use the PanelManager to show the Fail Panel
        if (panelManager != null)
        {
            panelManager.ShowFailPanel(); // Call the method to show the Fail Panel
        }
        else
        {
            Debug.LogError("PanelManager is not assigned or found!");
        }

        // Pause the game
        Time.timeScale = 0f;

        MarkGameOver();
    }

    private System.Collections.IEnumerator WaitForAdditionalPanelToFinish(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Mark the game as over after the additional panel is hidden
        MarkGameOver();
    }

    private void MarkGameOver()
    {
        isGameOver = true; // Mark the game as over
        Debug.Log("Game is now over.");
    }

    // Method to retry the current scene
    public void Retry()
    {
        Debug.Log("Retrying the scene...");

        // Unpause the game
        Time.timeScale = 1f;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Method to finish or exit
    public void Finish()
    {
        Debug.Log("Finishing the game...");

        // Use the PanelManager to show the Finish Panel
        if (panelManager != null)
        {
            panelManager.ShowFinishPanel();
        }

        // Pause the game
        Time.timeScale = 0f;
    }
}

/* using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaymerManagerHexafall : MonoBehaviour
{
    public Image timerImage; // Reference to the TimerImage
    public float totalTime = 60f; // Total time in seconds
    private float remainingTime;

    private PanelManager panelManager; // Reference to the PanelManager

    public GameObject player; // Reference to the player GameObject
    public float fallThreshold = -10f; // Y-axis threshold for detecting a fall

    private bool isGameOver = false; // Tracks if the game is already over
    public AdditionalPanelManager additionalPanelManager; // Reference to the AdditionalPanelManager
    private void Start()
    {
        remainingTime = totalTime; // Initialize the remaining time
        timerImage.fillAmount = 1f; // Start with a full bar

        // Find the PanelManager in the scene
        panelManager = FindObjectOfType<PanelManager>();

        if (panelManager == null)
        {
            Debug.LogError("PanelManager not found in the scene!");
        }

        // Ensure the game is not paused at the start
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (!isGameOver)
        {
            // Decrease the timer
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime; // Decrease the remaining time
                timerImage.fillAmount = remainingTime / totalTime; // Update the fill amount
            }
            else
            {
                HandleTimeOut(); // Call the method to handle timeout when time runs out
            }

            // Check if the player has fallen
            if (player != null && player.transform.position.y < fallThreshold)
            {
                HandlePlayerFall(); // Handle the player's fall
            }
        }
    }

    private void HandleTimeOut()
    {
        Debug.Log("Time's up! Showing Finish Panel.");

        // Use the PanelManager to show the Finish Panel
        if (panelManager != null)
        {
            panelManager.ShowFinishPanel(); // Call the method to show the Finish Panel
        }
        else
        {
            Debug.LogError("PanelManager is not assigned or found!");
        }

        // Show the additional panel
        if (additionalPanelManager != null)
        {
            additionalPanelManager.ShowAdditionalPanel();
            StartCoroutine(WaitForAdditionalPanelToFinish(1f)); // Wait for the additional panel to finish
        }
        else
        {
            Debug.LogError("AdditionalPanelManager is not assigned!");
            MarkGameOver(); // If no additional panel, mark the game as over immediately
        }

        // Stop the timer from continuing to decrease
        remainingTime = 0;

        // Pause the game
        Time.timeScale = 0f;

        isGameOver = true; // Mark the game as over
    }

    private void HandlePlayerFall()
    {
        Debug.Log("Player has fallen! Showing Fail Panel.");

        // Use the PanelManager to show the Fail Panel
        if (panelManager != null)
        {
            panelManager.ShowFailPanel(); // Call the method to show the Fail Panel
        }
        else
        {
            Debug.LogError("PanelManager is not assigned or found!");
        }

        // Pause the game
        Time.timeScale = 0f;

        //  isGameOver = true; // Mark the game as over
        MarkGameOver();
    }

    private System.Collections.IEnumerator WaitForAdditionalPanelToFinish(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Mark the game as over after the additional panel is hidden
        MarkGameOver();
    }

    private void MarkGameOver()
    {
        isGameOver = true; // Mark the game as over
        Debug.Log("Game is now over.");
    }

    // Method to retry the current scene
    public void Retry()
    {
        Debug.Log("Retrying the scene...");

        // Unpause the game
        Time.timeScale = 1f;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Method to finish or exit
    public void Finish()
    {
        Debug.Log("Finishing the game...");

        // Use the PanelManager to show the Finish Panel
        if (panelManager != null)
        {
            panelManager.ShowFinishPanel();
        }

        // Pause the game
        Time.timeScale = 0f;
    }
}

 */



/* using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaymerManagerHexafall : MonoBehaviour
{
    public Image timerImage; // Reference to the TimerImage
    public float totalTime = 60f; // Total time in seconds
    private float remainingTime;

    private PanelManager panelManager; // Reference to the PanelManager

    private void Start()
    {
        remainingTime = totalTime; // Initialize the remaining time
        timerImage.fillAmount = 1f; // Start with a full bar

        // Find the PanelManager in the scene
        panelManager = FindObjectOfType<PanelManager>();

        if (panelManager == null)
        {
            Debug.LogError("PanelManager not found in the scene!");
        }

        // Ensure the game is not paused at the start
        Time.timeScale = 1f;
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
            HandleTimeOut(); // Call the method to handle timeout when time runs out
        }
    }

    private void HandleTimeOut()
    {
        Debug.Log("Time's up! Showing fail panel.");

        // Use the PanelManager to show the fail panel
        panelManager?.ShowFailPanel();

        // Pause the game
        Time.timeScale = 0f;

        // Stop the timer from continuing to decrease
        remainingTime = 0;
    }

    // Method to retry the current scene
    public void Retry()
    {
        Debug.Log("Retrying the scene...");

        // Unpause the game
        Time.timeScale = 1f;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Method to finish or exit
    public void Finish()
    {
        Debug.Log("Finishing the game...");

        // Use the PanelManager to show the finish panel
        panelManager?.ShowFinishPanel();

        // Pause the game
        Time.timeScale = 0f;
    }
} */
/* using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaymerManagerSandbag : MonoBehaviour
{
    public Image timerImage; // Reference to the TimerImage
    public float totalTime = 60f; // Total time in seconds
    private float remainingTime;

    public GameObject failPanel; // Reference to the fail panel (UI for retry/finish options)

    private void Start()
    {
        remainingTime = totalTime; // Initialize the remaining time
        timerImage.fillAmount = 1f; // Start with a full bar

        // Ensure the fail panel is hidden at the start
        if (failPanel != null)
        {
            failPanel.SetActive(false);
        }

        // Ensure the game is not paused at the start
        Time.timeScale = 1f;
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
            HandleTimeOut(); // Call the method to handle timeout when time runs out
        }
    }

    private void HandleTimeOut()
    {
        Debug.Log("Time's up! Showing fail panel.");

        // Show the fail panel
        if (failPanel != null)
        {
            failPanel.SetActive(true);
        }

        // Pause the game
        Time.timeScale = 0f;

        // Stop the timer from continuing to decrease
        remainingTime = 0;
    }

    // Method to retry the current scene
    public void Retry()
    {
        Debug.Log("Retrying the scene...");

        // Unpause the game
        Time.timeScale = 1f;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Method to finish or exit
    public void Finish()
    {
        Debug.Log("Finishing the game...");

        // Unpause the game
        Time.timeScale = 1f;

        // Add logic here to transition to another scene or exit
        SceneManager.LoadScene("MainMenu"); // Example: Load the main menu scene
    }
} */