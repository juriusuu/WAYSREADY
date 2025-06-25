using UnityEngine;
using TMPro;
using System.Collections;


public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Timer Settings")]
    public float timeRemaining = 30f;
    public float penaltyTime = 5f;

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel;

    [Header("Game State")]
    public bool isGameActive = true;
    public ItemChecker itemChecker;
    private GoBagQuizManager goBagQuizManager; // Add reference to GoBagQuizManager

    [Header("End Panels")]
    public GameObject winPanel;
    public GameObject losePanel;
    public TMP_Text winScoreText;
    public TMP_Text loseScoreText;


    [Header("Feedback UI")]
    public CanvasGroup wrongItemCanvas;
    public float feedbackFadeDuration = 0.3f;
    public float feedbackDisplayTime = 1.2f;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Check if GoBagQuizManager is present in the scene
        goBagQuizManager = FindFirstObjectByType<GoBagQuizManager>();

        if (goBagQuizManager != null)
        {
            Debug.Log("GoBagQuizManager found - TimeManager will work in compatibility mode");
            // Let GoBagQuizManager handle the main game logic
            isGameActive = false; // Disable TimeManager's timer
        }
        else
        {
            Debug.Log("GoBagQuizManager not found - TimeManager will handle game logic");
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false); // Hide Game Over UI initially

        UpdateTimerText(); // Set initial time display
    }

    void Update()
    {
        if (!isGameActive) return;

        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            EndGame(false); // Time ran out
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString();
        }
        else
        {
            Debug.LogWarning("Timer Text is not assigned in TimeManager.");
        }
    }

    public void ApplyPenalty()
    {
        timeRemaining -= penaltyTime;
        if (timeRemaining < 0f)
            timeRemaining = 0f;

        UpdateTimerText();
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

        if (!didWin && gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void HandleItemDrop(bool isCorrect)
    {
        // If GoBagQuizManager is present, let it handle ALL the logic including wrong item feedback
        if (goBagQuizManager != null)
        {
            Debug.Log("Delegating item drop to GoBagQuizManager - TimeManager won't show feedback");
            return; // GoBagQuizManager will handle this completely
        }

        // Original TimeManager logic for backward compatibility (only when GoBagQuizManager not present)
        if (!isGameActive) return;

        if (isCorrect)
        {
            if (itemChecker != null)
            {
                itemChecker.ItemDropped(true);
            }
            else
            {
                Debug.LogWarning("ItemChecker is not assigned in TimeManager.");
            }
        }
        else
        {
            ApplyPenalty();

            // Show the wrong item feedback UI (only if GoBagQuizManager not present)
            if (wrongItemCanvas != null)
                StartCoroutine(ShowWrongItemFeedback());
        }
    }


    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }



    private Coroutine feedbackRoutine;

    private IEnumerator ShowWrongItemFeedback()
    {
        if (feedbackRoutine != null)
            StopCoroutine(feedbackRoutine);

        feedbackRoutine = StartCoroutine(FadeWrongItemFeedback());
        yield return null;
    }

    private IEnumerator FadeWrongItemFeedback()
    {
        // Fade in
        yield return StartCoroutine(FadeCanvasGroup(wrongItemCanvas, 0f, 1f, feedbackFadeDuration));

        // Wait
        yield return new WaitForSeconds(feedbackDisplayTime);

        // Fade out
        yield return StartCoroutine(FadeCanvasGroup(wrongItemCanvas, 1f, 0f, feedbackFadeDuration));
    }


    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0f;
        cg.alpha = start;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }

        cg.alpha = end;
    }

}
