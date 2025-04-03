using System.Collections.Generic; // Required for Dictionary
using UnityEngine;

using System.Collections.Generic; // Required for Dictionary
using UnityEngine;
using System.IO; // Required for file operations
using Newtonsoft.Json; // Install Newtonsoft.Json via Unity Package Manager

public class InventoryManagers : MonoBehaviour
{
    private static InventoryManagers _instance; // Singleton instance
    private Dictionary<string, int> itemInventory = new Dictionary<string, int>(); // Dictionary to hold item types and their quantities
    private int totalItemsToCollect = 11; // Total number of unique items to collect
    private int coinCount = 0; // Total coins collected
    private string saveFilePath; // Path to save the game data

    public static InventoryManagers Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("InventoryManager instance is null!");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance alive across scenes
            saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
            Debug.Log($"Save file path: {saveFilePath}");
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    public void AddItem(string itemName, int quantity)
    {
        Debug.Log($"AddItem called for: {itemName}, Quantity: {quantity}");

        if (itemInventory.ContainsKey(itemName))
        {
            itemInventory[itemName] += quantity;
            Debug.Log($"Updated {itemName} in inventory. New total: {itemInventory[itemName]}");
        }
        else
        {
            itemInventory[itemName] = quantity;
            Debug.Log($"Added new item {itemName} with quantity: {quantity}");
        }

        Debug.Log($"Total items in inventory after addition: {GetTotalItemCount()}");

        // Check if the total item count matches the required number
        if (GetTotalItemCount() >= totalItemsToCollect)
        {
            Debug.Log("All required items have been collected!");

            // Notify the QuestClipboardManager
            QuestClipboardManager questClipboardManager = FindObjectOfType<QuestClipboardManager>();
            if (questClipboardManager != null)
            {
                questClipboardManager.CompleteTask(1); // Assuming "Prepare an Emergency Kit" is the second task
            }

            // Reward coins for completing the task
            // AddCoins(50); // Reward 50 coins

            // Save the game state
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            SaveGame(currentScene);

            // Optionally, transition to the next scene
            //   FindObjectOfType<SceneTransitionManager>()?.TransitionToScene("NextSceneName");
        }
    }

    public int GetTotalItemCount()
    {
        int totalCount = 0;
        foreach (var item in itemInventory)
        {
            totalCount += item.Value; // Sum up all item quantities
        }
        return totalCount;
    }

    public int GetItemCount(string itemName)
    {
        int count = itemInventory.ContainsKey(itemName) ? itemInventory[itemName] : 0; // Return count or 0 if not found
        Debug.Log($"Current count for {itemName}: {count}");
        return count;
    }

    public void ResetInventory()
    {
        itemInventory.Clear(); // Clear the inventory
        Debug.Log("Inventory has been reset.");
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

    public void SetCoinCount(int count)
    {
        coinCount = count;
        CoinUIManager.Instance?.UpdateCoinUI(coinCount);
    }

    public Dictionary<string, int> GetInventory()
    {
        return new Dictionary<string, int>(itemInventory);
    }

    public void SetInventory(Dictionary<string, int> inventory)
    {
        itemInventory = new Dictionary<string, int>(inventory);
    }

    public void SaveGame(string currentScene)
    {
        SaveData saveData = new SaveData
        {
            currentScene = currentScene,
            coinCount = coinCount,
            inventory = itemInventory
        };

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        File.WriteAllText(saveFilePath, json);

        Debug.Log("Game saved successfully.");
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);

            // Restore the saved data
            coinCount = saveData.coinCount;
            itemInventory = new Dictionary<string, int>(saveData.inventory);

            // Load the saved scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(saveData.currentScene);

            Debug.Log("Game loaded successfully.");
        }
        else
        {
            Debug.LogWarning("No save file found.");
        }
    }

    public void DisplayInventory()
    {
        Debug.Log("Current Inventory:");
        foreach (var item in itemInventory)
        {
            Debug.Log($"{item.Key}: {item.Value}");

            // Find the GameObject for the item and enable its SpriteRenderer
            GameObject itemObject = GameObject.Find(item.Key);
            if (itemObject != null)
            {
                SpriteRenderer spriteRenderer = itemObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = true; // Enable the SpriteRenderer
                }
            }
        }
    }

    public void HideInventory()
    {
        Debug.Log("Hiding Inventory...");
        foreach (var item in itemInventory)
        {
            // Find the GameObject for the item and disable its SpriteRenderer
            GameObject itemObject = GameObject.Find(item.Key);
            if (itemObject != null)
            {
                SpriteRenderer spriteRenderer = itemObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = false; // Disable the SpriteRenderer
                }
            }
        }
    }
}

[System.Serializable]
public class SaveData
{
    public string currentScene; // The name of the current scene
    public int coinCount; // The player's total coins
    public Dictionary<string, int> inventory; // The player's inventory
}
/* 
public class InventoryManagers : MonoBehaviour
{
    private static InventoryManagers _instance; // Singleton instance
    private Dictionary<string, int> itemInventory = new Dictionary<string, int>(); // Dictionary to hold item types and their quantities
    private int totalItemsToCollect = 11; // Total number of unique items to collect

    private int coinCount = 0;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance alive across scenes
            Debug.Log("InventoryManager instance created.");
        }
        else
        {
            Debug.LogWarning("Another instance of InventoryManager was destroyed.");
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    public static InventoryManagers Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("InventoryManager instance is null!");
            }
            return _instance;
        }
    }

    public void AddItem(string itemName, int quantity)
    {
        Debug.Log($"AddItem called for: {itemName}, Quantity: {quantity}");

        if (itemInventory.ContainsKey(itemName))
        {
            itemInventory[itemName] += quantity;
            Debug.Log($"Updated {itemName} in inventory. New total: {itemInventory[itemName]}");
        }
        else
        {
            itemInventory[itemName] = quantity;
            Debug.Log($"Added new item {itemName} with quantity: {quantity}");
        }

        Debug.Log($"Total items in inventory after addition: {GetTotalItemCount()}");

        //Represents the Clipboard
        // Check if the total item count matches the required number
        if (GetTotalItemCount() >= totalItemsToCollect)
        {
            Debug.Log("All required items have been collected!");

            // Notify the QuestClipboardManager
            QuestClipboardManager questClipboardManager = FindObjectOfType<QuestClipboardManager>();
            if (questClipboardManager != null)
            {
                questClipboardManager.CompleteTask(1); // Assuming "Prepare an Emergency Kit" is the second task
            }
        }

    }


    public int GetTotalItemCount()
    {
        int totalCount = 0;
        foreach (var item in itemInventory)
        {
            totalCount += item.Value; // Sum up all item quantities
        }
        return totalCount;
    }

    public int GetItemCount(string itemName)
    {
        int count = itemInventory.ContainsKey(itemName) ? itemInventory[itemName] : 0; // Return count or 0 if not found
        Debug.Log($"Current count for {itemName}: {count}");
        return count;
    }

    public void ResetInventory()
    {
        itemInventory.Clear(); // Clear the inventory
        Debug.Log("Inventory has been reset.");
    }


    public void DisplayInventory()
    {
        Debug.Log("Current Inventory:");
        foreach (var item in itemInventory)
        {
            Debug.Log($"{item.Key}: {item.Value}");

            // Find the GameObject for the item and enable its SpriteRenderer
            GameObject itemObject = GameObject.Find(item.Key);
            if (itemObject != null)
            {
                SpriteRenderer spriteRenderer = itemObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = true; // Enable the SpriteRenderer
                }
            }
        }
    }
    public void HideInventory()
    {
        Debug.Log("Hiding Inventory...");
        foreach (var item in itemInventory)
        {
            // Find the GameObject for the item and disable its SpriteRenderer
            GameObject itemObject = GameObject.Find(item.Key);
            if (itemObject != null)
            {
                SpriteRenderer spriteRenderer = itemObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = false; // Disable the SpriteRenderer
                }
            }
        }
    }
} */