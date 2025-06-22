using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // <-- Add this line
public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public float timeRemaining = 30f;
    public float penaltyTime = 5f;
    public TextMeshProUGUI timerText;
    public bool isGameActive = true;

    public GameObject gameOverPanel; // Assign in Inspector

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {    // 👉 Add this block at the very top of Start()
        if (PlayerPrefs.GetInt("QuizDone_GoBag", 0) == 1)
        {
            isGameActive = false;
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);

                // Optionally hide Retry and show Complete
                Transform retryBtn = gameOverPanel.transform.Find("Retry");
                Transform completeBtn = gameOverPanel.transform.Find("Complete");

                if (retryBtn != null) retryBtn.gameObject.SetActive(false);
                if (completeBtn != null) completeBtn.gameObject.SetActive(true);
            }
            return; // Stop further initialization
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false); // Hide Game Over panel at start

        UpdateTimerText(); // Initialize timer display
    }

    void Update()
    {
        if (!isGameActive) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            EndGame(false); // Call this once when time runs out
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
            timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString();
    }

    public void ApplyPenalty()
    {
        timeRemaining -= penaltyTime;
        if (timeRemaining < 0)
            timeRemaining = 0;
        UpdateTimerText(); // Reflect the penalty
    }

    public void StopTimer()
    {
        isGameActive = false;
    }

    public void EndGame(bool didWin)
    {
        if (!isGameActive) return; // Prevent multiple calls

        isGameActive = false;
        Debug.Log(didWin ? "You win!" : "Time's up! You lose!");

        /*  if (!didWin && gameOverPanel != null)
             gameOverPanel.SetActive(true); */

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            // Find buttons by name or reference
            Transform retryBtn = gameOverPanel.transform.Find("Retry");
            Transform completeBtn = gameOverPanel.transform.Find("Complete");

            if (retryBtn != null) retryBtn.gameObject.SetActive(!didWin);
            if (completeBtn != null) completeBtn.gameObject.SetActive(didWin);
        }
    }
    public float rewardTime = 3f; // Add this at the top with your other public variables

    /*   public void HandleItemDrop(bool isCorrect)
      {
          if (!isGameActive) return;

          if (isCorrect)
          {
              timeRemaining += rewardTime; // Add time for correct item
              UpdateTimerText();           // Update the timer display
              FindObjectOfType<ItemChecker>().ItemDropped(true);
          }
          else
          {
              ApplyPenalty(); // Deduct time for wrong items
          }
      } */
    public void HandleItemDrop(ItemData droppedItem)
    {
        if (!isGameActive) return;

        if (droppedItem != null && droppedItem.isRequired)
        {
            timeRemaining += rewardTime; // Add time for correct item
            UpdateTimerText();           // Update the timer display
            FindObjectOfType<ItemChecker>().ItemDropped(droppedItem);
        }
        else
        {
            ApplyPenalty(); // Deduct time for wrong items
        }
    }
    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CompleteGame()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Save completion flag
        PlayerPrefs.SetInt("QuizDone_GoBag", 1);
        PlayerPrefs.Save();
        // Load the main menu scene (replace "Main Menu" with your actual main menu scene name)
        SceneManager.LoadScene("Main Menu");
    }


}