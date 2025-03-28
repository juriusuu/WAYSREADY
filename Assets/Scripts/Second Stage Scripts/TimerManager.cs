using UnityEngine;
using TMPro; // Include this for TextMeshPro
using System.Collections; // Include this for IEnumerator
using UnityEngine.SceneManagement; // Include this for scene management


public enum GameState
{
    Playing,
    PlayerDead,
    StageCompleted
}

public class TimerManager : MonoBehaviour
{
    public float countdownTime; // Set the initial countdown time to 60 seconds
    public TMP_Text timerText; // Reference to the UI Text for the timer

    private float timer;
    private GameState currentState;

    void Start()
    {
        currentState = GameState.Playing; // Initialize the state to Playing
        StartCountdown(); // Start the countdown when the game starts
    }

    public void SetCountdownTime(float time)
    {
        countdownTime = time; // Set the countdown time based on difficulty
        StartCountdown(); // Restart the countdown with the new time
    }

    public void StartCountdown()
    {
        timer = this.countdownTime; // Reset the timer to the countdown time
        StartCoroutine(CountdownCoroutine()); // Start the countdown coroutine
    }

    private IEnumerator CountdownCoroutine()
    {
        while (timer > 0 && currentState == GameState.Playing)
        {
            timer -= Time.deltaTime; // Decrease the timer
            UpdateTimerUI(); // Update the timer UI
            yield return null; // Wait for the next frame
        }

        if (currentState == GameState.Playing)
        {
            timer = 0; // Ensure timer does not go below 0
            UpdateTimerUI(); // Update the UI to show 0
            HandleTimerEnd(); // Handle what happens when the timer ends
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time Left: " + Mathf.Ceil(timer).ToString(); // Update the text to show remaining time
        }
    }

    private void HandleTimerEnd()
    {
        // Get the current active scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Check the current scene and set the timer accordingly
        switch (currentSceneName)
        {
            case "FirstSceneEasy":
                Debug.Log("Time's up in FirstScene (Easy)! Player has died.");
                TransitionToState(GameState.PlayerDead);
                GameManager.Instance.Die();
                break;

            case "FirstSceneMedium":
                Debug.Log("Time's up in FirstScene (Medium)! Player has completed the stage.");
                TransitionToState(GameState.StageCompleted);
                break;

            case "FirstSceneHard":
                Debug.Log("Time's up in FirstScene (Hard)! Player has died.");
                TransitionToState(GameState.PlayerDead);
                GameManager.Instance.Die();
                break;

            case "Second Stage Easy":
                SetCountdownTime(180f); // 180 seconds for Second Stage Easy
                Debug.Log("Starting Second Stage Easy with 180 seconds.");
                break;

            case "Second Stage Medium":
                SetCountdownTime(120f); // 120 seconds for Second Stage Medium
                Debug.Log("Starting Second Stage Medium with 120 seconds.");
                break;

            case "Second Stage Hard":
                SetCountdownTime(60f); // 60 seconds for Second Stage Hard
                Debug.Log("Starting Second Stage Hard with 60 seconds.");
                break;

                // Add more cases for other scenes as needed
        }
    }
    private void TransitionToState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.PlayerDead:
                Debug.Log("Player has died.");
                // Handle player death logic
                break;

            case GameState.StageCompleted:
                Debug.Log("Player has passed the stage.");
                // Handle stage completion logic
                break;
        }
    }

    // Add the StartRespawnCoroutine method
    public void StartRespawnCoroutine(LifeManager lifeManager)
    {
        StartCoroutine(RespawnCoroutine(lifeManager));
    }

    private IEnumerator RespawnCoroutine(LifeManager lifeManager)
    {
        // Add your respawn logic here
        yield return new WaitForSeconds(3); // Example wait time
        lifeManager.Respawn(); // Call the Respawn method in LifeManager
    }
}
/* 
    private void HandleTimerEnd()
    {
        // Get the current active scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Get the current difficulty level from the DifficultyManager
        DifficultyManager.DifficultyLevel currentDifficulty = DifficultyManager.Instance.currentDifficulty;

        // Check the current scene and difficulty level to handle accordingly
        if (currentSceneName == "FirstSceneEasy")
        {
            Debug.Log("Time's up in FirstScene (Easy)! Player has died.");
            TransitionToState(GameState.PlayerDead);
            GameManager.Instance.Die();
        }
        else if (currentSceneName == "FirstSceneMedium")
        {
            Debug.Log("Time's up in FirstScene (Medium)! Player has completed the stage.");
            TransitionToState(GameState.StageCompleted);
            // Call your stage completion logic here
        }
        else if (currentSceneName == "FirstSceneHard")
        {
            Debug.Log("Time's up in FirstScene (Hard)! Player has died.");
            TransitionToState(GameState.PlayerDead);
            GameManager.Instance.Die();
        }
        else if (currentSceneName == "Second Stage Easy")
        {
            Debug.Log("Time's up in SecondScene (Easy)! Player has completed the stage.");
            TransitionToState(GameState.StageCompleted);
            // Call your stage completion logic here
        }
        else if (currentSceneName == "Second Stage Medium")
        {
            Debug.Log("Time's up in SecondScene (Medium)! Player has died.");
            TransitionToState(GameState.PlayerDead);
            GameManager.Instance.Die();
        }
        else if (currentSceneName == "Second Stage Hard")
        {
            Debug.Log("Time's up in SecondScene (Hard)! Player has died.");
            TransitionToState(GameState.PlayerDead);
            GameManager.Instance.Die();
        }
        else if (currentSceneName == "ThirdSceneEasy")
        {
            Debug.Log("Time's up in ThirdScene (Easy)! Player has completed the stage.");
            TransitionToState(GameState.StageCompleted);
            // Call your stage completion logic here
        }
        else if (currentSceneName == "ThirdSceneMedium")
        {
            Debug.Log("Time's up in ThirdScene (Medium)! Player has died.");
            TransitionToState(GameState.PlayerDead);
            GameManager.Instance.Die();
        }
        else if (currentSceneName == "ThirdSceneHard")
        {
            Debug.Log("Time's up in ThirdScene (Hard)! Player has died.");
            TransitionToState(GameState.PlayerDead);
            GameManager.Instance.Die();
        }
    }
 */


//Update March 17
/* public enum GameState
{
    Playing,
    PlayerDead,
    StageCompleted
}

public class TimerManager : MonoBehaviour
{
    public float countdownTime = 60f; // Set the initial countdown time to 60 seconds
    public TMP_Text timerText; // Reference to the UI Text for the timer

    private float timer;
    private GameState currentState;

    void Start()
    {
        currentState = GameState.Playing; // Initialize the state to Playing
        StartCountdown(); // Start the countdown when the game starts
    }

    public void StartCountdown()
    {
        timer = this.countdownTime; // Reset the timer to the countdown time
        StartCoroutine(CountdownCoroutine()); // Start the countdown coroutine
    }

    private IEnumerator CountdownCoroutine()
    {
        while (timer > 0 && currentState == GameState.Playing)
        {
            timer -= Time.deltaTime; // Decrease the timer
            UpdateTimerUI(); // Update the timer UI
            yield return null; // Wait for the next frame
        }

        if (currentState == GameState.Playing)
        {
            timer = 0; // Ensure timer does not go below 0
            UpdateTimerUI(); // Update the UI to show 0
            HandleTimerEnd(); // Handle what happens when the timer ends
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time Left: " + Mathf.Ceil(timer).ToString(); // Update the text to show remaining time
        }
    }

    private void HandleTimerEnd()
    {
        if (SceneManager.GetActiveScene().name == "FirstScene")
        {
            TransitionToState(GameState.PlayerDead);
        }
        else if (SceneManager.GetActiveScene().name == "SecondScene")
        {
            TransitionToState(GameState.StageCompleted);
        }// lalagay ka ng else if dito depende kung anong gusto mong mangyari kapag naubos ung oras mo gagayahin mo lang ung code sa taas tapos lalagay mo ung name ng scene mo then under that ung
    }

    private void TransitionToState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.PlayerDead:
                Debug.Log("Player has died.");
                GameManager.Instance.Die(); // Call the Die method from GameManager
                break;

            case GameState.StageCompleted:
                Debug.Log("Player has passed the stage.");
                // Call your stage completion logic here, e.g., LoadNextStage();
                break;
        }
    }

    // Add the StartRespawnCoroutine method
    public void StartRespawnCoroutine(LifeManager lifeManager)
    {
        StartCoroutine(RespawnCoroutine(lifeManager));
    }

    private IEnumerator RespawnCoroutine(LifeManager lifeManager)
    {
        // Add your respawn logic here
        yield return new WaitForSeconds(3); // Example wait time
        lifeManager.Respawn(); // Call the Respawn method in LifeManager
    }
} */
/* Without GameState
public class TimerManager : MonoBehaviour
{
    public float countdownTime = 60f; // Set the initial countdown time to 60 seconds
    public TMP_Text timerText; // Reference to the UI Text for the timer

    private float timer;

    void Start()
    {
        StartCountdown(); // Start the countdown when the game starts
    }

    public void StartCountdown()
    {
        timer = this.countdownTime; // Reset the timer to the countdown time
        StartCoroutine(CountdownCoroutine()); // Start the countdown coroutine
    }

    private IEnumerator CountdownCoroutine()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime; // Decrease the timer
            UpdateTimerUI(); // Update the timer UI
            yield return null; // Wait for the next frame
        }
        timer = 0; // Ensure timer does not go below 0
        UpdateTimerUI(); // Update the UI to show 0
        // Optionally, trigger an event when the timer reaches zero
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time Left: " + Mathf.Ceil(timer).ToString(); // Update the text to show remaining time
        }
    }

    public void StartRespawnCoroutine(LifeManager lifeManager)
    {
        StartCoroutine(RespawnCoroutine(lifeManager));
    }

    private IEnumerator RespawnCoroutine(LifeManager lifeManager)
    {
        // Add your respawn logic here
        yield return new WaitForSeconds(3); // Example wait time
        lifeManager.Respawn();
    }
}
 */