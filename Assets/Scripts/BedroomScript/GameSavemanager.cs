using System.IO;
using UnityEngine;
using Newtonsoft.Json; // Install Newtonsoft.Json via Unity Package Manager
using System.Collections.Generic;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance; // Singleton instance

    private string saveFilePath;
    private int coinCount = 0;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        Debug.Log($"Save file path: {saveFilePath}");
    }

    public void RewardAndSave(int coinsToAdd)
    {
        AddCoins(coinsToAdd);
        SaveGame();
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        Debug.Log($"Added {amount} coins. Total coins: {coinCount}");

        // Update the UI
        if (CoinUIManager.Instance != null)
        {
            CoinUIManager.Instance.UpdateCoinUI(coinCount);
        }
        else
        {
            Debug.LogWarning("CoinUIManager instance is null. Unable to update coin UI.");
        }
    }

    public int GetCoinCount()
    {
        return coinCount;
    }
    public void SaveGame()
    {
        try
        {
            // Extract inventory data
            var inventory = InventoryManagers.Instance.GetInventory();
            Dictionary<string, int> inventoryQuantities = new Dictionary<string, int>();
            Dictionary<string, string> inventorySprites = new Dictionary<string, string>();

            foreach (var item in inventory)
            {
                inventoryQuantities[item.Key] = item.Value.quantity;
                inventorySprites[item.Key] = item.Value.sprite != null ? item.Value.sprite.name : null;
            }

            // Create save data
            SaveData saveData = new SaveData
            {
                currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                coinCount = coinCount,
                inventory = inventoryQuantities,
                itemSprites = inventorySprites
            };

            // Serialize and save to file
            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            File.WriteAllText(saveFilePath, json);

            Debug.Log("Game saved successfully.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to save game: {ex.Message}");
        }
    }
    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);

                // Restore coin count
                coinCount = saveData.coinCount;

                // Reconstruct inventory
                Dictionary<string, (int quantity, Sprite sprite)> inventory = new Dictionary<string, (int, Sprite)>();
                foreach (var item in saveData.inventory)
                {
                    string itemName = item.Key;
                    int quantity = item.Value;
                    Sprite sprite = null;

                    if (saveData.itemSprites.ContainsKey(itemName) && !string.IsNullOrEmpty(saveData.itemSprites[itemName]))
                    {
                        sprite = Resources.Load<Sprite>($"Sprites/{saveData.itemSprites[itemName]}");
                    }

                    inventory[itemName] = (quantity, sprite);
                }

                InventoryManagers.Instance.SetInventory(inventory);

                // Load the saved scene
                UnityEngine.SceneManagement.SceneManager.LoadScene(saveData.currentScene);

                Debug.Log("Game loaded successfully.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load game: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning("No save file found.");
        }
    }

    private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

    private Sprite GetSprite(string spriteName)
    {
        if (!spriteCache.ContainsKey(spriteName))
        {
            Sprite sprite = Resources.Load<Sprite>($"Sprites/{spriteName}");
            if (sprite != null)
            {
                spriteCache[spriteName] = sprite;
            }
            else
            {
                Debug.LogWarning($"Sprite not found: {spriteName}");
            }
        }
        return spriteCache[spriteName];
    }
    [System.Serializable]
    public class SaveData
    {
        public string currentScene;
        public int coinCount;
        public Dictionary<string, int> inventory; // For saving only quantities
        public Dictionary<string, string> itemSprites; // For saving sprite names
    }
}
/* 
using System.IO;
using UnityEngine;
using Newtonsoft.Json; // Install Newtonsoft.Json via Unity Package Manager


using System.Collections.Generic;


public class GameSaveManager : MonoBehaviour
{
    private string saveFilePath;
    private int coinCount = 0;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        Debug.Log($"Save file path: {saveFilePath}");
    }

    public void RewardAndSave(int coinsToAdd)
    {
        AddCoins(coinsToAdd);
        SaveGame();
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        Debug.Log($"Added {amount} coins. Total coins: {coinCount}");

        // Update the UI
        if (CoinUIManager.Instance != null)
        {
            CoinUIManager.Instance.UpdateCoinUI(coinCount);
        }
        else
        {
            Debug.LogWarning("CoinUIManager instance is null. Unable to update coin UI.");
        }
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

    public void SaveGame()
    {
        try
        {
            SaveData saveData = new SaveData
            {
                currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                coinCount = coinCount,
                inventory = InventoryManagers.Instance.GetInventory()
            };

            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            File.WriteAllText(saveFilePath, json);

            Debug.Log("Game saved successfully.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to save game: {ex.Message}");
        }
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);

            // Restore the saved data
            coinCount = saveData.coinCount;
            InventoryManagers.Instance.SetInventory(saveData.inventory);

            // Load the saved scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(saveData.currentScene);

            Debug.Log("Game loaded successfully.");
        }
        else
        {
            Debug.LogWarning("No save file found.");
        }
    }
}

[System.Serializable]
public class SaveData
{
    public string currentScene;
    public int coinCount;
    public Dictionary<string, int> inventory;
} */
/* 
public class GameSaveManager : MonoBehaviour
{
    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        Debug.Log($"Save file path: {saveFilePath}");
    }

    // Save the game state

         public void SaveGame()
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
        } 
   

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
 */