using System.Collections.Generic;
using UnityEngine;

public class InstructionManagerSandBag : MonoBehaviour
{
    public GameObject instructionPanel;
    public List<GameObject> pages;
    public GameObject nextButton;
    public GameObject backButton;
    public GameObject startButton;

    private int currentPage = 0;
    private StageDialogueManager1 stageDialogueManager;
    public TaymerManagerSandbag taymerManager; // Fixed: Use TaymerManagerSandbag instead of TaymerManagerHexafall

    void Awake() // Use Awake to run before other Start() methods
    {
        // Find managers
        stageDialogueManager = FindFirstObjectByType<StageDialogueManager1>();
        taymerManager = FindFirstObjectByType<TaymerManagerSandbag>();

        // Pause everything at the very beginning
        Time.timeScale = 0f;

        // Ensure instruction panel is active
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
        }
    }

    void Start()
    {
        ShowPage(currentPage);
        Debug.Log("Instructions started - game is paused");
    }

    void ShowPage(int index)
    {
        Debug.Log("Showing Page: " + index);

        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == index);
        }

        backButton.SetActive(index > 0);
        nextButton.SetActive(index < pages.Count - 1);
        startButton.SetActive(index == pages.Count - 1);
    }

    public void NextPage()
    {
        if (currentPage < pages.Count - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }

    public void OnBackClicked()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
    }

    public void OnStartClicked()
    {
        instructionPanel.SetActive(false);

        // Start the dialogue first
        if (stageDialogueManager != null)
        {
            stageDialogueManager.StartDialogue();
        }

        // Start the timer AFTER dialogue is triggered
        if (taymerManager != null)
        {
            taymerManager.StartGame();
        }

        // Resume the game
        Time.timeScale = 1f;
        Debug.Log("Instructions completed, dialogue and timer started!");
    }
}