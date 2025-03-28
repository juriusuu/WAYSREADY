using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void GoToGameMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Load the game selection scene
    }
}