using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadFirstStage()
    {
        SceneManager.LoadScene("First Stage"); // Load the first stage scene
    }

    public void LoadSecondStage()
    {
        SceneManager.LoadScene("Second Stage"); // Load the second stage scene
    }
}