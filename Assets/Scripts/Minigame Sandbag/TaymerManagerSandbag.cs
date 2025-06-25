using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaymerManagerSandbag : MonoBehaviour
{
    public Image timerImage; // Reference to the TimerImage
    public float totalTime = 60f; // Total time in seconds
    private float remainingTime;

    private PanelManager panelManager; // Reference to the PanelManager
    public AdditionalPanelManager additionalPanelManager; // Reference to the AdditionalPanelManager

    // Remove all instruction system fields - they belong in InstructionManagerSandBag
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
        // Only run the timer if instructions are completed AND game has started
        if (instructionsCompleted && gameStarted && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime; // Decrease the remaining time
            timerImage.fillAmount = remainingTime / totalTime; // Update the fill amount
        }
        else if (instructionsCompleted && gameStarted && remainingTime <= 0)
        {
            HandleTimeOut(); // Call the method to handle timeout when time runs out
        }
    }

    // Call this method from InstructionManagerSandBag when instructions are done
    public void StartGame()
    {
        instructionsCompleted = true;
        gameStarted = true;
        Debug.Log("Game started! Timer is now running.");
    }

    private void HandleTimeOut()
    {
        Debug.Log("Time's up! Showing fail panel.");
        panelManager?.ShowFailPanel();
        Time.timeScale = 0f;
        remainingTime = 0;
    }

    public void Retry()
    {
        Debug.Log("Retrying the scene...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Finish()
    {
        Debug.Log("Finishing the game...");
        panelManager?.ShowFinishPanel();

        if (additionalPanelManager != null)
        {
            additionalPanelManager.ShowAdditionalPanel();
            Debug.Log("AdditionalPanelManager activated.");
        }
        else
        {
            Debug.LogError("AdditionalPanelManager is not assigned in the Inspector!");
        }

        Time.timeScale = 0f;
    }
}

/* using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TaymerManagerSandbag : MonoBehaviour
{
    public Image timerImage; // Reference to the TimerImage
    public float totalTime = 60f; // Total time in seconds
    private float remainingTime;

    private PanelManager panelManager; // Reference to the PanelManager
    public AdditionalPanelManager additionalPanelManager; // Reference to the AdditionalPanelManager

    [Header("Instruction System")]
    public GameObject instructionPanel;
    public List<GameObject> pages;
    public GameObject nextButton;
    public GameObject backButton;
    public GameObject startButton;

    private int currentPage = 0;
    private bool gameStarted = false; // Track if the game has started
    private bool instructionsCompleted = false; // NEW: Track if instructions are done

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
        // Only run the timer if instructions are completed AND game has started
        if (instructionsCompleted && gameStarted && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime; // Decrease the remaining time
            timerImage.fillAmount = remainingTime / totalTime; // Update the fill amount
        }
        else if (instructionsCompleted && gameStarted && remainingTime <= 0)
        {
            HandleTimeOut(); // Call the method to handle timeout when time runs out
        }
    }

    // Call this method from InstructionManagerSandBag when instructions are done
    public void StartGame()
    {
        instructionsCompleted = true;
        gameStarted = true;
        Debug.Log("Game started! Timer is now running.");
    }

    // Rest of your existing methods...
    private void HandleTimeOut()
    {
        Debug.Log("Time's up! Showing fail panel.");
        panelManager?.ShowFailPanel();
        Time.timeScale = 0f;
        remainingTime = 0;
    }

    public void Retry()
    {
        Debug.Log("Retrying the scene...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Finish()
    {
        Debug.Log("Finishing the game...");
        panelManager?.ShowFinishPanel();

        if (additionalPanelManager != null)
        {
            additionalPanelManager.ShowAdditionalPanel();
            Debug.Log("AdditionalPanelManager activated.");
        }
        else
        {
            Debug.LogError("AdditionalPanelManager is not assigned in the Inspector!");
        }

        Time.timeScale = 0f;
    }
}
 */

/* using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaymerManagerSandbag : MonoBehaviour
{
    public Image timerImage; // Reference to the TimerImage
    public float totalTime = 60f; // Total time in seconds
    private float remainingTime;

    private PanelManager panelManager; // Reference to the PanelManager
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


        // Call the AdditionalPanelManager
        if (additionalPanelManager != null)
        {
            additionalPanelManager.ShowAdditionalPanel();
            Debug.Log("AdditionalPanelManager activated.");
        }
        else
        {
            Debug.LogError("AdditionalPanelManager is not assigned in the Inspector!");
        }

        // Pause the game
        Time.timeScale = 0f;
    }
}
 */






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