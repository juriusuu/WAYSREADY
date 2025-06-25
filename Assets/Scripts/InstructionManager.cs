using System.Collections.Generic;
using UnityEngine;

public class InstructionManager : MonoBehaviour
{
    public GameObject instructionPanel;
    public List<GameObject> pages;
    public GameObject nextButton;
    public GameObject backButton;
    public GameObject startButton;



    private int currentPage = 0;

    void Start()
    {
        ShowPage(currentPage);
        Time.timeScale = 0f; // ❗️Pause game at start
    }

void ShowPage(int index)
{
    Debug.Log("Showing Page: " + index); // 🔍

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
        Time.timeScale = 1f; // ❗️Resume game when starting
    }
}
