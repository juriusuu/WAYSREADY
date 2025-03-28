using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMain : MonoBehaviour
{
    public void LoadGameSelection()
    {
        SceneManager.LoadScene("Stage 1 Living Room"); // Load the first stage scene
        Debug.Log("Main Game Stage 1 Living Room Loaded");
    }


    public void LoadGameSelectionSecondStage()
    {
        SceneManager.LoadScene("Stage 2 Classroom"); // Load the 2nd stage scene
        Debug.Log("Main Game Stage 2 Classroom Loaded");
    }



    public void LoadGameSelectionThirdStage()
    {
        SceneManager.LoadScene("Stage 3"); // Load the 3rd stage scene
        Debug.Log("Main Game Stage 3");
    }




    public void LoadMiniGame()
    {
        SceneManager.LoadScene("First Stage Easy"); // Load the mini game scene
        Debug.Log("Mini Game Loaded");
    }


    public void LoadMiniGameHexa()
    {
        SceneManager.LoadScene("Second Stage Medium"); // Load the mini game scene
        Debug.Log("Mini Game Loaded");
    }


    public void LoadMiniGameMaze()
    {
        SceneManager.LoadScene("Easy Maze"); // Load the mini game scene
        Debug.Log("Mini Game Loaded");
    }
}