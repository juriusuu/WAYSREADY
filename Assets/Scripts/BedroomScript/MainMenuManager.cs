using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        // Check if a panel needs to be activated
        if (!string.IsNullOrEmpty(QuestClipboardManager.panelToActivate))
        {
            // Find the panel by name, even if it's inactive
            GameObject panel = FindPanelByName(QuestClipboardManager.panelToActivate);
            if (panel != null)
            {
                // Activate the panel
                panel.SetActive(true);
                Debug.Log($"Activated panel: {QuestClipboardManager.panelToActivate}");
            }
            else
            {
                Debug.LogError($"Panel '{QuestClipboardManager.panelToActivate}' not found in the Main Menu scene.");
            }

            // Clear the static variable after use
            QuestClipboardManager.panelToActivate = null;
        }
    }

    private GameObject FindPanelByName(string panelName)
    {
        // Search for all GameObjects, including inactive ones
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == panelName && obj.scene.isLoaded) // Ensure it's in the current scene
            {
                return obj;
            }
        }
        return null;
    }
}