using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro; // Enable TextMeshPro support

public class GoBagQuizManager : MonoBehaviour
{
    [Header("Game Setup")]
    public ItemData[] items;
    public Transform dragItemsParent;
    public GameObject dragItemPrefab;

    [Header("Game UI")]
    public GameObject winPanel;
    public GameObject losePanel;
    public CanvasGroup wrongItemPanel; // Changed to CanvasGroup for fade effects
    public TextMeshProUGUI winScoreText;     // Changed to TextMeshPro
    public TextMeshProUGUI loseScoreText;    // Changed to TextMeshPro
    public TextMeshProUGUI scoreText;        // Changed to TextMeshPro
    public TextMeshProUGUI timerText;        // Changed to TextMeshPro

    [Header("Feedback Settings")]
    public float feedbackFadeDuration = 0.3f;
    public float feedbackDisplayTime = 1.2f;

    [Header("Game Controls")]
    public Button restartButton;
    public Button mainMenuButton;
    public Button winRestartButton;
    public Button winMainMenuButton;

    [Header("Game Settings")]
    public float gameTimer = 120f; // 2 minutes

    private int score = 0;
    private float timer;
    private bool isGameActive = true;
    private bool isPausedForFeedback = false; // Track if paused for wrong item feedback
    private List<ItemData> requiredItems;
    private List<ItemData> droppedItems;
    private Coroutine feedbackCoroutine;

    void Start()
    {
        InitializeGame();
        SetupUIButtons();
        SpawnDragItems();
    }

    void Update()
    {
        // Don't update timer if game is not active or paused for feedback
        if (!isGameActive || isPausedForFeedback) return;

        timer -= Time.deltaTime;
        UpdateTimerUI();

        if (timer <= 0)
        {
            timer = 0;
            GameOver();
        }
    }

    void InitializeGame()
    {
        timer = gameTimer;
        score = 0;
        droppedItems = new List<ItemData>();

        // Get only required items
        requiredItems = items.Where(item => item.isRequired).ToList();

        UpdateScoreUI();
        UpdateTimerUI();
    }

    void SetupUIButtons()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (winRestartButton != null)
            winRestartButton.onClick.AddListener(RestartGame);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(LoadMainMenu);

        if (winMainMenuButton != null)
            winMainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    void SpawnDragItems()
    {
        foreach (var item in items)
        {
            GameObject go = Instantiate(dragItemPrefab, dragItemsParent);
            DragItem di = go.GetComponent<DragItem>();
            if (di != null)
            {
                di.itemData = item;
            }

            // Set icon and name
            Image iconImage = go.GetComponent<Image>();
            if (iconImage != null && item.icon != null)
            {
                iconImage.sprite = item.icon;
            }

            Text label = go.GetComponentInChildren<Text>();
            if (label == null)
            {
                // Try TextMeshPro if regular Text not found
                TextMeshProUGUI tmpLabel = go.GetComponentInChildren<TextMeshProUGUI>();
                if (tmpLabel != null)
                {
                    tmpLabel.text = item.itemName;
                }
            }
            else
            {
                label.text = item.itemName;
            }

            // 🎯 Randomize local position within the panel's area
            RectTransform parentRect = dragItemsParent.GetComponent<RectTransform>();
            RectTransform itemRect = go.GetComponent<RectTransform>();

            float panelWidth = parentRect.rect.width;
            float panelHeight = parentRect.rect.height;

            float randomX = Random.Range(0, panelWidth - itemRect.rect.width);
            float randomY = Random.Range(0, panelHeight - itemRect.rect.height);

            itemRect.anchoredPosition = new Vector2(randomX, -randomY);
        }
    }

    // Public method to be called when an item is successfully dropped
    public void OnItemDropped(ItemData item)
    {
        if (!isGameActive) return;

        if (item.isRequired && !droppedItems.Contains(item))
        {
            droppedItems.Add(item);
            score += 10; // Points for correct item
            UpdateScoreUI();

            Debug.Log($"Correct item dropped: {item.itemName}");
            CheckWinCondition();
        }
    }

    // Public method to be called when a wrong item is dropped
    public void OnWrongItemDropped(ItemData item)
    {
        if (!isGameActive)
        {
            Debug.Log("Game is not active - ignoring wrong item drop");
            return;
        }

        Debug.Log($"OnWrongItemDropped called for: {item.itemName}");

        // 1. Apply penalties first (minus time and score)
        score -= 5; // Penalty for wrong item
        timer -= 5f; // Time penalty
        if (timer < 0) timer = 0;

        Debug.Log($"Score changed to: {score}, Timer changed to: {timer}");

        // 2. Update UI to show the changes immediately
        UpdateScoreUI();

        // 3. Pause game and show wrong item panel with fade effect
        StartWrongItemFeedback();

        Debug.Log($"Wrong item dropped: {item.itemName}");
    }

    void CheckWinCondition()
    {
        if (droppedItems.Count >= requiredItems.Count)
        {
            GameComplete();
        }
    }

    void GameComplete()
    {
        /*  Debug.Log("Congratulations! All required items collected!");
         isGameActive = false;

         // Calculate time bonus (commented out - no time bonus)
         // int timeBonus = Mathf.RoundToInt(timer * 2);
         // int finalScore = score + timeBonus;

         winPanel.SetActive(true);
         if (winScoreText != null)
             winScoreText.text = $"Final Score: {score}";

         // score = finalScore; // No longer needed since no time bonus */
        Debug.Log("Congratulations! All required items collected!");
        isGameActive = false;

        int finalScore = score;

        winPanel.SetActive(true);
        if (winScoreText != null)
            winScoreText.text = $"Score: {finalScore}";

        // Save high score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (finalScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
            PlayerPrefs.Save();
            Debug.Log("New high score saved!");
        }

        // Update the actual score
        score = finalScore;
    }

    void GameOver()
    {
        Debug.Log("Time's up! Game Over!");
        isGameActive = false;
        losePanel.SetActive(true);

        if (loseScoreText != null)
            loseScoreText.text = $"Final Score: {score}";
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
            // Debug.Log($"Timer updated: {minutes:00}:{seconds:00}"); // Uncomment for debugging
        }
        else
        {
            Debug.LogError("TimerText is null! Please assign it in the inspector.");
        }
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

    void StartWrongItemFeedback()
    {
        if (wrongItemPanel == null)
        {
            Debug.LogError("WrongItemPanel (CanvasGroup) is null! Please assign it in the inspector.");
            return;
        }

        Debug.Log("Starting wrong item feedback - pausing game");

        // Pause the game
        isPausedForFeedback = true;

        // Stop any existing feedback coroutine
        if (feedbackCoroutine != null)
            StopCoroutine(feedbackCoroutine);

        // Start the feedback sequence
        feedbackCoroutine = StartCoroutine(WrongItemFeedbackSequence());
    }

    private System.Collections.IEnumerator WrongItemFeedbackSequence()
    {
        Debug.Log("Wrong item feedback sequence started");

        // Fade in the wrong item panel
        yield return StartCoroutine(FadeCanvasGroup(wrongItemPanel, 0f, 1f, feedbackFadeDuration));

        // Display the panel for the specified time
        yield return new WaitForSeconds(feedbackDisplayTime);

        // Fade out the wrong item panel
        yield return StartCoroutine(FadeCanvasGroup(wrongItemPanel, 1f, 0f, feedbackFadeDuration));

        // Resume the game
        isPausedForFeedback = false;
        Debug.Log("Wrong item feedback sequence completed - resuming game");
    }

    private System.Collections.IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = startAlpha;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaled time so it works even when paused
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
