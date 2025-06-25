using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswers> QnA;
    public GameObject[] options;
    public int currentQuestion;

    public GameObject Quizpanel;
    public GameObject GoPanel;

    public Text QuestionTxt;
    public Text ScoreTxt;
    public Text TimerTxt; // Assign this in Inspector

    public float timePerQuestion = 10f;
    private float currentTime;
    private bool isTimerRunning = false;

    int totalQuestions = 0;
    public int score;
    public Button CompleteButton; // Assign in Inspector

    public CanvasGroup wrongAnswerCanvas;
    public float feedbackFadeDuration = 0.3f;
    public float feedbackDisplayTime = 1.2f;
    private Coroutine feedbackRoutine;

    private void Start()
    {
        totalQuestions = QnA.Count;
        GoPanel.SetActive(false);
        if (CompleteButton != null)
            CompleteButton.interactable = false; // Disable by default
        generateQuestion();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            TimerTxt.text = Mathf.Ceil(currentTime).ToString();

            if (currentTime <= 0)
            {
                isTimerRunning = false;
                wrong(); // count as wrong if time runs out
            }
        }
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void GameOver()
    {
        Quizpanel.SetActive(false);
        GoPanel.SetActive(true);
        ScoreTxt.text = score + "/" + totalQuestions;

        if (CompleteButton != null)
            CompleteButton.interactable = score > 5; // Enable only if score > 5
    }

    public void correct()
    {
        score += 1;
        isTimerRunning = false;
        QnA.RemoveAt(currentQuestion);
        generateQuestion();
    }
    public void Complete()
    {
        // Save assessment completion (using PlayerPrefs as an example)
        PlayerPrefs.SetInt("AssessmentCompleted", 1);
        PlayerPrefs.Save();

        // Load main menu (replace "MainMenu" with your actual main menu scene name)
        SceneManager.LoadScene("Main Menu");
    }
    public void wrong()
    {
        isTimerRunning = false;
        QnA.RemoveAt(currentQuestion);

        // Show feedback UI
        if (wrongAnswerCanvas != null)
            StartCoroutine(ShowWrongAnswerFeedback());

        generateQuestion();
    }

    private IEnumerator ShowWrongAnswerFeedback()
    {
        if (feedbackRoutine != null)
            StopCoroutine(feedbackRoutine);

        feedbackRoutine = StartCoroutine(FadeWrongAnswerFeedback());
        yield return null;
    }

    private IEnumerator FadeWrongAnswerFeedback()
    {
        // Fade in
        yield return StartCoroutine(FadeCanvasGroup(wrongAnswerCanvas, 0f, 1f, feedbackFadeDuration));

        // Wait
        yield return new WaitForSeconds(feedbackDisplayTime);

        // Fade out
        yield return StartCoroutine(FadeCanvasGroup(wrongAnswerCanvas, 1f, 0f, feedbackFadeDuration));
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


    void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestion].Answers[i];

            if (QnA[currentQuestion].CorrectAnswer == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void generateQuestion()
    {
        if (QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);
            QuestionTxt.text = QnA[currentQuestion].Question;
            SetAnswers();

            currentTime = timePerQuestion;
            isTimerRunning = true;
        }
        else
        {
            Debug.Log("Out of Questions");
            GameOver();
        }
    }
}
