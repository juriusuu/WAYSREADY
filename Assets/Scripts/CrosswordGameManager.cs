using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class CrosswordGameManager : MonoBehaviour
{
    [Header("Input UI")]
    public TMP_InputField answerInputField;

    [Header("Grid Setup")]
    public GameObject cellPrefab;
    public Transform gridParent;
    public int gridSize = 10;

    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public GameObject clueTextPrefab;
    public Transform clueContainer;

    [Header("Word List")]
    public List<CrosswordWord> wordList = new List<CrosswordWord>();

    [Header("Clue Sections")]
    public TMP_Text acrossHeader;
    public TMP_Text downHeader;
    public Transform acrossClueContainer;
    public Transform downClueContainer;



    [Header("End Panels")]
    public GameObject winPanel;
    public GameObject losePanel;
    public TMP_Text winScoreText;
    public TMP_Text loseScoreText;
    public Button restartButton;
    public Button mainMenuButton;
    public Button winRestartButton;
    public Button winMainMenuButton;

    [Header("Feedback UI")]
    public CanvasGroup wrongAnswerCanvas;
    public float feedbackFadeDuration = 0.3f;
    public float feedbackDisplayTime = 1.2f;

    [Header("Game State")]
    public Button pauseButton;
    public GameObject pausePanel;
    public Button resumeButton;
    public Button pauseMainMenuButton;

    [Header("Hint System")]
    public Button hintButton;
    public int maxHints = 3;
    private int hintsUsed = 0;
    public TMP_Text hintCountText;

    private CrosswordCell[,] cells;
    private float timer = 60f;
    private int score = 0;
    private bool isPlaying = true;
    private bool isPaused = false;



    void Start()
    {
        // Initialize button listeners
        SetupUIButtons();

        GenerateGrid();
        ShowClues();
        PlaceWords();
        UpdateScoreText();
        UpdateHintCountText();
    }

    void Update()
    {
        if (!isPlaying || isPaused) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 0;
            isPlaying = false;
            timerText.text = $"Time: 00:00";
            GameOver(); // Call a game over handler
            return;
        }

        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = $"Time: {minutes:00}:{seconds:00}";
    }


    void GenerateGrid()
    {
        cells = new CrosswordCell[gridSize, gridSize];

        for (int r = 0; r < gridSize; r++)
        {
            for (int c = 0; c < gridSize; c++)
            {
                GameObject obj = Instantiate(cellPrefab, gridParent);
                CrosswordCell cell = obj.GetComponent<CrosswordCell>();
                cell.Init(r, c);
                cells[r, c] = cell;
            }
        }
    }

    void PlaceWords()
    {
        foreach (var word in wordList)
        {
            bool canPlace = true;

            for (int i = 0; i < word.word.Length; i++)
            {
                int r = word.startRow + (word.isHorizontal ? 0 : i);
                int c = word.startCol + (word.isHorizontal ? i : 0);

                if (r >= gridSize || c >= gridSize)
                {
                    canPlace = false;
                    break;
                }

                char newChar = char.ToUpper(word.word[i]);
                var cell = cells[r, c];

                if (cell.IsUsed && cell.GetAnswerChar() != newChar.ToString())
                {
                    canPlace = false;
                    break;
                }
            }

            if (!canPlace)
            {
                Debug.LogWarning($"Skipped placing word: {word.word}");
                continue;
            }

            // Place word
            for (int i = 0; i < word.word.Length; i++)
            {
                int r = word.startRow + (word.isHorizontal ? 0 : i);
                int c = word.startCol + (word.isHorizontal ? i : 0);
                cells[r, c].SetAnswerChar(word.word[i]);
                cells[word.startRow, word.startCol].SetLegendNumber(word.clueNumber);

            }
        }
    }


    public void UseHint()
    {
        if (!isPlaying || wordList.Count == 0 || hintsUsed >= maxHints) return;

        foreach (var word in wordList)
        {
            string correct = word.word.ToUpper();

            for (int i = 0; i < correct.Length; i++)
            {
                int r = word.startRow + (word.isHorizontal ? 0 : i);
                int c = word.startCol + (word.isHorizontal ? i : 0);

                CrosswordCell cell = cells[r, c];

                if (string.IsNullOrEmpty(cell.letterText.text))
                {
                    cell.RevealChar(correct[i]);

                    score -= 10;
                    timer -= 5f;

                    if (timer < 0) timer = 0;

                    hintsUsed++;
                    UpdateScoreText();
                    UpdateHintCountText();
                    return;
                }
            }
        }

        Debug.Log("No hints available — all words are already filled.");
    }

    private void UpdateHintCountText()
    {
        if (hintCountText != null)
            hintCountText.text = $"Hints: {maxHints - hintsUsed}/{maxHints}";
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {score}";
    }


    public void SubmitAnswer()
    {
        string typedAnswer = answerInputField.text.Trim().ToUpper();
        CrosswordWord match = wordList.Find(w => w.word.ToUpper() == typedAnswer);

        if (match == null)
        {
            score -= 10;
            timer -= 5f; // ✅ Added time penalty for wrong answers

            if (timer < 0) timer = 0; // Prevent negative time

            UpdateScoreText();
            ShowWrongAnswerFeedback(); // ✅ Show panel only if answer is wrong
            return;
        }

        score += 10;

        for (int i = 0; i < match.word.Length; i++)
        {
            int r = match.startRow + (match.isHorizontal ? 0 : i);
            int c = match.startCol + (match.isHorizontal ? i : 0);
            cells[r, c].letterText.text = match.word[i].ToString();
        }

        if (match.clueObject != null)
        {
            Destroy(match.clueObject);
        }

        answerInputField.text = "";
        wordList.Remove(match);
        UpdateScoreText();
        CheckWinCondition();
    }

    void ShowClues()
    {
        // Clear containers
        foreach (Transform child in acrossClueContainer) Destroy(child.gameObject);
        foreach (Transform child in downClueContainer) Destroy(child.gameObject);

        int acrossCount = 1;
        int downCount = 1;

        foreach (var word in wordList)
        {
            GameObject clueObj = Instantiate(clueTextPrefab);
            TMP_Text clueText = clueObj.GetComponent<TMP_Text>();

            if (word.isHorizontal)
            {
                word.clueNumber = acrossCount;
                clueText.text = $"{acrossCount}. {word.clue}";
                clueObj.transform.SetParent(acrossClueContainer, false);
                acrossCount++;
            }
            else
            {
                word.clueNumber = downCount;
                clueText.text = $"{downCount}. {word.clue}";
                clueObj.transform.SetParent(downClueContainer, false);
                downCount++;
            }

            word.clueObject = clueObj;
        }

        acrossHeader.text = "Across";
        downHeader.text = "Down";
    }

    void GameOver()
    {
        Debug.Log("Time's up! Game Over!");
        isPlaying = false;
        losePanel.SetActive(true);
        loseScoreText.text = $"Final Score: {score}";

        // Optional: Save high score or game statistics here
        // PlayerPrefs.SetInt("LastScore", score);
    }

    void CheckWinCondition()
    {
        if (wordList.Count == 0)
        {
            isPlaying = false;
            GameComplete();
        }
    }

    void GameComplete()
    {
        /*  Debug.Log("Congratulations! All words completed!");

         // Calculate bonus points before showing the panel
         int timeBonus = Mathf.RoundToInt(timer * 2); // 2 points per second remaining
         int finalScore = score + timeBonus;

         winPanel.SetActive(true);
         winScoreText.text = $"Score: {score} + {timeBonus} = {finalScore}";

         // Update the actual score
         score = finalScore;

         // Optional: Save high score
         // int highScore = PlayerPrefs.GetInt("HighScore", 0);
         // if (score > highScore) 
         // {
         //     PlayerPrefs.SetInt("HighScore", score);
         //     // Show new high score message
         // } */
        Debug.Log("Congratulations! All words completed!");

        int finalScore = score;

        winPanel.SetActive(true);
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


    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    private Coroutine feedbackRoutine;

    public void ShowWrongAnswerFeedback()
    {
        if (wrongAnswerCanvas == null) return;

        if (feedbackRoutine != null)
            StopCoroutine(feedbackRoutine);

        feedbackRoutine = StartCoroutine(FadeWrongAnswerFeedback());
    }

    private IEnumerator FadeWrongAnswerFeedback()
    {
        yield return StartCoroutine(FadeCanvasGroup(wrongAnswerCanvas, 0f, 1f, feedbackFadeDuration));
        yield return new WaitForSeconds(feedbackDisplayTime);
        yield return StartCoroutine(FadeCanvasGroup(wrongAnswerCanvas, 1f, 0f, feedbackFadeDuration));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvas, float start, float end, float duration)
    {
        float elapsed = 0f;
        canvas.alpha = start;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }

        canvas.alpha = end;
    }
    void SetupUIButtons()
    {
        // Setup restart buttons
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (winRestartButton != null)
            winRestartButton.onClick.AddListener(RestartGame);

        // Setup main menu buttons
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(LoadMainMenu);

        if (winMainMenuButton != null)
            winMainMenuButton.onClick.AddListener(LoadMainMenu);

        // Setup pause buttons
        if (pauseButton != null)
            pauseButton.onClick.AddListener(PauseGame);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (pauseMainMenuButton != null)
            pauseMainMenuButton.onClick.AddListener(PauseToMainMenu);

        // Setup hint button
        if (hintButton != null)
            hintButton.onClick.AddListener(UseHint);
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    public void PauseToMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale before loading scene
        LoadMainMenu();
    }

}