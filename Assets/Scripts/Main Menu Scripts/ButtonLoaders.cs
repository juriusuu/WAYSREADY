using UnityEngine;
using UnityEngine.SceneManagement; // Make sure to include this namespace

public class ButtonLoaders : MonoBehaviour
{
    // Method to load the first stage on Easy difficulty
    public void LoadFirstStageEasy()
    {
        SceneManager.LoadScene("Stage 1 Living Room"); // Replace with the actual scene name
    }

    // Method to load the first stage on Medium difficulty
    public void LoadFirstStageMedium()
    {
        SceneManager.LoadScene("Stage 2 Classroom"); // Replace with the actual scene name
    }

    // Method to load the first stage on Hard difficulty
    public void LoadFirstStageHard()
    {
        SceneManager.LoadScene("Stage 3"); // Replace with the actual scene name
    }
}