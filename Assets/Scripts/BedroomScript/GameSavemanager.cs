using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json; // Install Newtonsoft.Json via Unity Package Manager


using System.Collections.Generic;

public class GameSaveManager : MonoBehaviour
{
    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        Debug.Log($"Save file path: {saveFilePath}");
    }

    // Save the game state

    /*     public void SaveGame()
        {
            try
            {
                SaveData saveData = new SaveData
                {
                    currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                    coinCount = InventoryManagers.Instance.GetCoinCount(),
                    inventory = InventoryManagers.Instance.GetInventory()
                };

                string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
                File.WriteAllText(saveFilePath, json);

                Debug.Log($"Game saved successfully. Coin count saved: {saveData.coinCount}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save game: {ex.Message}");
            }
        } */
    /*     public void SaveGame()
        {
            SaveData saveData = new SaveData
            {
                currentScene = SceneManager.GetActiveScene().name, // Save the current scene
                coinCount = InventoryManagers.Instance.GetCoinCount(), // Save the coin count
                inventory = InventoryManagers.Instance.GetInventory() // Save the inventory
            };

            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            File.WriteAllText(saveFilePath, json);

            Debug.Log("Game saved successfully.");
        } */

    // Load the game state
    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);

            // Load the saved scene
            SceneManager.LoadScene(saveData.currentScene);

            // Restore the coin count and inventory after the scene is loaded
            StartCoroutine(RestoreGameData(saveData));
        }
        else
        {
            Debug.LogWarning("No save file found.");
        }
    }

    private System.Collections.IEnumerator RestoreGameData(SaveData saveData)
    {
        // Wait for the scene to load
        yield return new WaitForSeconds(0.1f);

        // Restore the coin count and inventory
        InventoryManagers.Instance.SetCoinCount(saveData.coinCount);
        InventoryManagers.Instance.SetInventory(saveData.inventory);

        Debug.Log("Game loaded successfully.");
    }
}

namespace GameSaveSystem
{
    [System.Serializable]
    public class SaveData
    {
        public string currentScene; // The name of the current scene
        public int coinCount; // The player's total coins
        public Dictionary<string, int> inventory; // The player's inventory
    }
}
