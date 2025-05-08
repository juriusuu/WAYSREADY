/* using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameButtonHandler : MonoBehaviour
{
    public void OnNewGameButtonClick()
    {
        // Clear PlayerPrefs (removes all saved data stored in PlayerPrefs)
        PlayerPrefs.DeleteAll();

        // Delete the save file if it exists
        string saveFilePath = Path.Combine(Application.persistentDataPath, "SavedGameWR.json");
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted successfully.");
        }

        // Reset the GameManager state
        if (GameManager.Instance != null)
        {
            GameManager.Instance.coinCount = 0;
            GameManager.Instance.completedScenes.Clear();
            GameManager.Instance.questCompletionStatus.Clear();
            GameManager.Instance.sceneStates.Clear();
            GameManager.Instance.additionalTime = 0;
            GameManager.Instance.additionalLives = 0;
            GameManager.Instance.additionalHints = 0;
            Debug.Log("GameManager state reset successfully.");
        }


    }
} */

using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameButtonHandler : MonoBehaviour
{
    public void OnNewGameButtonClick()
    {
        // Clear PlayerPrefs (removes all saved data stored in PlayerPrefs)
        PlayerPrefs.DeleteAll();

        // Delete the save file if it exists
        string saveFilePath = Path.Combine(Application.persistentDataPath, "SavedGameWR.json");
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted successfully.");
        }

        // Reset the GameManager state
        if (GameManager.Instance != null)
        {
            GameManager.Instance.coinCount = 0;
            GameManager.Instance.completedScenes.Clear();
            GameManager.Instance.questCompletionStatus.Clear();
            GameManager.Instance.sceneStates.Clear();
            GameManager.Instance.additionalTime = 0;
            GameManager.Instance.additionalLives = 0;
            GameManager.Instance.additionalHints = 0;
            Debug.Log("GameManager state reset successfully.");
        }

        // Notify the StageSelectionManager to reset its state
        StageSelectionManager stageSelectionManager = FindObjectOfType<StageSelectionManager>();
        if (stageSelectionManager != null)
        {
            stageSelectionManager.ResetProgress();
            Debug.Log("StageSelectionManager progress reset successfully.");
        }
        else
        {
            Debug.LogWarning("StageSelectionManager not found in the scene.");
        }


        // Notify the StageButtonActivator to reset trophies
        StageButtonActivator stageButtonActivator = FindObjectOfType<StageButtonActivator>();
        if (stageButtonActivator != null)
        {
            stageButtonActivator.ResetTrophies();
            Debug.Log("Trophies reset successfully.");
        }
        else
        {
            Debug.LogWarning("StageButtonActivator not found in the scene.");
        }

        // Optionally, load the first scene or main menu
        // SceneManager.LoadScene("Main Menu"); // Replace "MainMenu" with your main menu scene name
    }
}