using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    // Game States
    public enum GameState
    {
        Playing,
        PlayerDead,
        GameOver
    }

    public GameState CurrentState { get; private set; } = GameState.Playing;

    // Save Data
    private string saveFilePath;
    public int coinCount = 0;
    private string currentScene; // Track the current scene
    public List<string> completedScenes = new List<string>(); // List of completed scenes
    private Dictionary<string, bool[]> questCompletionStatus = new Dictionary<string, bool[]>(); // Quest completion status
    private Dictionary<string, SceneState> sceneStates = new Dictionary<string, SceneState>(); // Tracks all scenes
    private Vector3 playerStartingPosition; // Store the player's starting position

    private Dictionary<string, ObjectState> objectStates = new Dictionary<string, ObjectState>();

    public int additionalTime; // Time purchased
    public int additionalLives = 0; // Lives purchased
    public int additionalHints = 0; // Hints purchased


    public Dictionary<string, float> defaultSceneTimes = new Dictionary<string, float>
    {
        { "Stage1Easy", 300f },
        { "Stage1Normal", 240f },
        { "Stage1Hard", 180f },
        { "Stage2Easy", 300f },
        { "Stage2Normal", 240f },
        { "Stage2Hard", 180f },
        { "Stage3Easy", 300f },
        { "Stage3Normal", 240f },
        { "Stage3Hard", 180f }
    };

    public Dictionary<string, int> defaultSceneHints = new Dictionary<string, int>
{
    { "Stage1Easy", 5 },
    { "Stage1Normal", 3 },
    { "Stage1Hard", 0 },
    { "Stage2Easy", 5 },
    { "Stage2Normal", 3 },
    { "Stage2Hard", 0 },
    { "Stage3Easy", 5 },
    { "Stage3Normal", 3 },
    { "Stage3Hard", 0 }
};

    public Dictionary<string, int> defaultSceneLives = new Dictionary<string, int>
{
    { "Stage1Easy", 2 },
    { "Stage1Normal", 3 },
    { "Stage1Hard", 1 },
    { "Stage2Easy", 2 },
    { "Stage2Normal", 3 },
    { "Stage2Hard", 1 },
    { "Stage3Easy", 2 },
    { "Stage3Normal", 3 },
    { "Stage3Hard", 1 }
};

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

        saveFilePath = Path.Combine(Application.persistentDataPath, "SavedGameWR.json");
        Debug.Log($"Save file path: {saveFilePath}");

        // Apply stored purchases early
        // ApplyStoredPurchases();


    }

    public float GetDefaultTimeForScene(string sceneName)
    {
        if (defaultSceneTimes.ContainsKey(sceneName))
        {
            return defaultSceneTimes[sceneName];
        }
        else
        {
            Debug.LogWarning($"Scene '{sceneName}' not found in defaultSceneTimes. Using fallback default time.");
            return 60f; // Fallback default time if the scene is not found
        }
    }
    public int GetDefaultLivesForScene(string sceneName)
    {
        if (defaultSceneLives.ContainsKey(sceneName))
        {
            return defaultSceneLives[sceneName];
        }
        else
        {
            Debug.LogWarning($"Scene '{sceneName}' not found in defaultSceneLives. Using fallback default lives.");
            return 3; // Fallback default lives
        }
    }
    public int GetDefaultHintsForScene(string sceneName)
    {
        if (defaultSceneHints.ContainsKey(sceneName))
        {
            return defaultSceneHints[sceneName];
        }
        else
        {
            Debug.LogWarning($"Scene '{sceneName}' not found in defaultSceneHints. Using fallback default hints.");
            return 0; // Fallback default hints if the scene is not found
        }
    }

    private void InitializePlayerStartingPosition()
    {
        // Find the player in the scene
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerStartingPosition = player.transform.position; // Store the player's starting position
            Debug.Log($"Player starting position initialized to: {playerStartingPosition}");
        }
        else
        {
            Debug.LogWarning("Player not found in the scene. Skipping starting position initialization.");
        }
    }

    private void Start()
    {
        // Get the current scene name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Check if the current scene is a player-dependent scene
        if (currentSceneName == "Main Menu" || currentSceneName == "Settings" || currentSceneName == "Credits")
        {
            Debug.Log($"Skipping player initialization in scene: {currentSceneName}");
            return; // Exit early for non-player scenes
        }


        // Initialize the player's starting position
        InitializePlayerStartingPosition();
        // Apply stored purchases after all managers are initialized
        //   ApplyStoredPurchases();

    }



    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    /* 
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"Scene loaded: {scene.name}. Applying stored purchases.");
            ApplyStoredPurchases();
            Debug.Log($"[GameManager] Scene loaded: {scene.name}");

            if (scene.name == "Main Menu")
            {
                Debug.Log("Checking for ShopManager in Main Menu.");
                ShopManager shopManager = FindObjectOfType<ShopManager>();
                if (shopManager != null)
                {
                    Debug.Log("ShopManager found. Reassigning ShopMenu reference.");
                    GameObject shopMenu = GameObject.Find("ShopMenu");
                    if (shopMenu != null)
                    {
                        shopManager.shopMenu = shopMenu;
                        Debug.Log("ShopMenu reference reassigned.");

                        // Reassign button references
                        shopManager.ReassignButtonEvents();

                        // Activate the ShopMenu if needed
                        shopMenu.SetActive(false); // Ensure it starts inactive
                        Debug.Log("ShopMenu is ready but inactive.");
                    }
                    else
                    {
                        Debug.LogWarning("ShopMenu not found in the Main Menu scene. It will be assigned dynamically when the shop is opened.");
                    }
                }
                else
                {
                    Debug.LogWarning("ShopManager not found in the scene. It will be initialized dynamically.");
                }
            }
        }
     */

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}. Applying stored purchases.");
        ApplyStoredPurchases();
        Debug.Log($"[GameManager] Scene loaded: {scene.name}");

        if (scene.name == "Main Menu")
        {
            Debug.Log("Main Menu loaded. Reassigning ShopMenu.");
            StartCoroutine(AssignShopMenuWithDelay());
        }
    }
    private System.Collections.IEnumerator AssignShopMenuWithDelay()
    {
        yield return new WaitForEndOfFrame(); // Wait for the end of the frame to ensure all objects are initialized

        ShopManager shopManager = ShopManager.Instance;
        if (shopManager != null)
        {
            Debug.Log("ShopManager found. Reassigning ShopMenu reference.");

            // Use Resources.FindObjectsOfTypeAll to find inactive objects
            GameObject shopMenu = null;
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (obj.name == "ShopMenu" && obj.hideFlags == HideFlags.None) // Ensure it's not hidden in the hierarchy
                {
                    shopMenu = obj;
                    break;
                }
            }

            if (shopMenu != null)
            {
                shopManager.shopMenu = shopMenu;
                Debug.Log("ShopMenu reference reassigned.");

                // Reassign button references
                shopManager.ReassignButtonEvents();

                // Ensure the ShopMenu starts inactive
                shopMenu.SetActive(false);
                Debug.Log("ShopMenu is ready but inactive.");

                // Reassign OnClick events for buttons
                shopManager.ReassignOnClickEvents();
            }
            else
            {
                Debug.LogError("ShopMenu not found in the scene!");
            }
        }
        else
        {
            Debug.LogWarning("ShopManager not found in the scene!");
        }
    }
    /* 
    private System.Collections.IEnumerator AssignShopMenuWithDelay()
    {
        yield return new WaitForEndOfFrame(); // Wait for the end of the frame to ensure all objects are initialized

        ShopManager shopManager = ShopManager.Instance;
        if (shopManager != null)
        {
            Debug.Log("ShopManager found. Reassigning ShopMenu reference.");

            // Use Resources.FindObjectsOfTypeAll to find inactive objects
            GameObject shopMenu = null;
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (obj.name == "ShopMenu" && obj.hideFlags == HideFlags.None) // Ensure it's not hidden in the hierarchy
                {
                    shopMenu = obj;
                    break;
                }
            }

            if (shopMenu != null)
            {
                shopManager.shopMenu = shopMenu;
                Debug.Log("ShopMenu reference reassigned.");

                // Reassign button references
                shopManager.ReassignButtonEvents();

                // Ensure the ShopMenu starts inactive
                shopMenu.SetActive(false);
                Debug.Log("ShopMenu is ready but inactive.");
            }
            else
            {
                Debug.LogError("ShopMenu not found in the scene!");
            }
        }
        else
        {
            Debug.LogWarning("ShopManager not found in the scene!");
        }
    } */
    /* 
        private System.Collections.IEnumerator AssignShopMenuWithDelay()
        {
            yield return new WaitForEndOfFrame(); // Wait for the end of the frame to ensure all objects are initialized

            ShopManager shopManager = ShopManager.Instance;
            if (shopManager != null)
            {
                Debug.Log("ShopManager found. Reassigning ShopMenu reference.");

                // Search for the ShopMenu in the scene hierarchy, including inactive objects
                Transform shopMenuTransform = GameObject.Find("Main Menu")?.transform.Find("ShopMenu");
                if (shopMenuTransform != null)
                {
                    GameObject shopMenu = shopMenuTransform.gameObject;
                    shopManager.shopMenu = shopMenu;
                    Debug.Log("ShopMenu reference reassigned.");

                    // Reassign button references
                    shopManager.ReassignButtonEvents();

                    // Ensure the ShopMenu starts inactive
                    shopMenu.SetActive(false);
                    Debug.Log("ShopMenu is ready but inactive.");
                }
                else
                {
                    Debug.LogError("ShopMenu not found in the Main Menu scene!");
                }
            }
            else
            {
                Debug.LogWarning("ShopManager not found in the scene!");
            }
        } */


    /* 
    private System.Collections.IEnumerator AssignShopMenuWithDelay()
    {
        yield return new WaitForEndOfFrame(); // Wait for the end of the frame to ensure all objects are initialized

        ShopManager shopManager = FindObjectOfType<ShopManager>();
        if (shopManager != null)
        {
            Debug.Log("ShopManager found. Reassigning ShopMenu reference.");
            if (shopManager.shopMenu != null)
            {
                Debug.Log("ShopMenu reference already assigned.");
                shopManager.ReassignButtonEvents();

                // Ensure the ShopMenu starts inactive
                shopManager.shopMenu.SetActive(false);
                Debug.Log("ShopMenu is ready but inactive.");
            }
            else
            {
                Debug.LogError("ShopMenu reference is missing in ShopManager! Please assign it in the Inspector.");
            }
        }
        else
        {
            Debug.LogWarning("ShopManager not found in the scene!");
        }
    } */
    /* 
        private System.Collections.IEnumerator AssignShopMenuWithDelay()
        {
            yield return new WaitForEndOfFrame(); // Wait for the end of the frame to ensure all objects are initialized

            ShopManager shopManager = FindObjectOfType<ShopManager>();
            if (shopManager != null)
            {
                Debug.Log("ShopManager found. Reassigning ShopMenu reference.");
                GameObject shopMenu = GameObject.Find("ShopMenu");
                if (shopMenu != null)
                {
                    shopManager.shopMenu = shopMenu;
                    Debug.Log("ShopMenu reference reassigned.");

                    // Reassign button references
                    shopManager.ReassignButtonEvents();

                    // Ensure the ShopMenu starts inactive
                    shopMenu.SetActive(false);
                    Debug.Log("ShopMenu is ready but inactive.");
                }
                else
                {
                    Debug.LogError("ShopMenu not found in the Main Menu scene!");
                }
            }
            else
            {
                Debug.LogWarning("ShopManager not found in the scene!");
            }
        } */
    public void ApplyStoredPurchases()

    {
        Debug.Log($"[GameManager] Applying stored purchases. Additional time: {additionalTime}");

        // Apply additional lives to LayfManager if present
        LayfManager layfManager = FindObjectOfType<LayfManager>();
        if (layfManager != null && additionalLives > 0)
        {
            Debug.Log($"Applying {additionalLives} stored lives to LayfManager.");
            for (int i = 0; i < additionalLives; i++)
            {
                layfManager.AddLife();
            }
            Debug.Log($"Applied {additionalLives} lives to LayfManager.");
            //  additionalLives = 0; // Reset after applying
            Debug.Log($"Applying {additionalLives} stored lives to LayfManager.");
        }

        // Apply additional time to TaymerManager if present
        TaymerManager taymerManager = FindObjectOfType<TaymerManager>();
        if (taymerManager != null && additionalTime > 0)
        {
            //taymerManager.AddTime(additionalTime);
            taymerManager.AddAdditionalTime(additionalTime);
            Debug.Log($"Applied {additionalTime} seconds to TaymerManager.");
            //  additionalTime = 0; // Reset after applying
            Debug.Log($"Applying stored purchases. Additional time: {additionalTime}, Additional lives: {additionalLives}, Additional hints: {additionalHints}");

        }


        // Apply additional hints to TaymerManager if present
        if (taymerManager != null && additionalHints > 0)
        {
            for (int i = 0; i < additionalHints; i++)
            {
                taymerManager.AddHint(); // Use AddHint instead of UseHint
            }
            Debug.Log($"Applied {additionalHints} hints to TaymerManager.");
            /*  additionalHints = 0; */ // Reset after applying
        }
    }

    // -------------------- Game State Management --------------------

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState)
        {
            Debug.LogWarning($"GameState is already {newState}. Ignoring redundant state change.");
            return; // Prevent redundant state changes
        }

        Debug.Log($"GameState changed from {CurrentState} to {newState}");
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Playing:
                OnEnterPlayingState();
                break;
            case GameState.PlayerDead:
                OnEnterPlayerDeadState();
                break;
            case GameState.GameOver:
                OnEnterGameOverState();
                break;
        }
    }
    public void AddRewardCoins(int amount)
    {
        // Add the reward to the player's coin count
        coinCount += amount;
        Debug.Log($"Rewarded {amount} coins. Total coins: {coinCount}");

        // Update the coin UI if it exists
        if (CoinUIManager.Instance != null)
        {


            CoinUIManager.Instance.UpdateCoinUI(coinCount);
        }
        else
        {
            Debug.LogWarning("CoinUIManager instance is null. Unable to update coin UI.");
        }
    }

    private void OnEnterPlayingState()
    {
        Debug.Log("Entered Playing state.");
        Time.timeScale = 1f; // Resume gameplay
    }
    private void OnEnterPlayerDeadState()
    {
        Debug.Log("Entered PlayerDead state.");
        Time.timeScale = 0f; // Pause gameplay

        // Reference the LayfManager and TaymerManager
        LayfManager layfManager = FindObjectOfType<LayfManager>();
        TaymerManager taymerManager = FindObjectOfType<TaymerManager>();

        if (layfManager != null)
        {
            layfManager.LoseLife(); // Reduce a life

            if (layfManager.GetRemainingLives() > 0) // Check if the player has lives left
            {
                Debug.Log("Player lost a life. Resetting the game state...");
                if (taymerManager != null)
                {
                    taymerManager.ResetTimer(); // Reset the timer in TaymerManager
                }

                ResetGameState(); // Reset the game state without reloading the scene
            }
            else
            {
                Debug.Log("No lives remaining. Transitioning to GameOver state...");
                ChangeState(GameState.GameOver); // Transition to GameOver state
            }
        }
        else
        {
            Debug.LogError("LayfManager not found! Unable to reduce life.");
        }
    }

    private void ResetGameState()
    {
        Debug.Log("Resetting game state...");

        // Reset player position (if needed)
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = playerStartingPosition; // Reset to the starting position
            Debug.Log($"Player position reset to starting position: {playerStartingPosition}");
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }
        // Resume gameplay
        Time.timeScale = 1f;
    }
    public void OnEnterGameOverState()
    {
        /*  Debug.Log("Entered GameOver state.");
         Time.timeScale = 0f; // Pause gameplay
         // Show Game Over UI */
        Debug.Log("Entered GameOver state.");
        Time.timeScale = 0f; // Pause gameplay

        // Reference the TaymerManager to access the fail panel
        TaymerManager taymerManager = FindObjectOfType<TaymerManager>();

        if (taymerManager != null && taymerManager.failPanel != null)
        {
            taymerManager.failPanel.SetActive(true); // Show the fail panel
            Debug.Log("Fail panel activated.");
        }
        else
        {
            Debug.LogError("Fail panel or TaymerManager not found!");
        }
    }

    // -------------------- Scene Management --------------------

    public void LoadScene(string sceneName)
    {
        Debug.Log($"Loading scene: {sceneName}");

        // Save the current scene state before transitioning
        SaveCurrentSceneState();

        // Update the current scene
        currentScene = sceneName;

        // Save the game to persist all scene states
        SaveGame(currentScene);

        // Load the new scene
        SceneManager.LoadScene(sceneName);

        // Reset to Playing state on scene load
        ChangeState(GameState.Playing);
    }

    public void ReloadCurrentScene()
    {
        Debug.Log($"Reloading current scene: {currentScene}");
        LoadScene(currentScene);
    }

    private void SaveCurrentSceneState()
    {
        if (string.IsNullOrEmpty(currentScene))
        {
            Debug.LogWarning("SaveCurrentSceneState: currentScene is null or empty. Skipping save.");
            return;
        }

        // Create a new SceneState object
        SceneState sceneState = new SceneState
        {
            isCompleted = completedScenes.Contains(currentScene),
            questStates = new Dictionary<string, bool[]>()
        };

        // Save quest states for the current scene
        foreach (var quest in questCompletionStatus)
        {
            sceneState.questStates[quest.Key] = quest.Value;
        }

        // Update the sceneStates dictionary
        if (sceneStates.ContainsKey(currentScene))
        {
            sceneStates[currentScene] = sceneState;
        }
        else
        {
            sceneStates.Add(currentScene, sceneState);
        }

        Debug.Log($"Scene state for '{currentScene}' saved successfully.");
    }

    // -------------------- Save and Load --------------------

    public void SaveGame(string currentScene = null)
    {
        try
        {
            if (!string.IsNullOrEmpty(currentScene))
            {
                this.currentScene = currentScene;
            }
            else
            {
                Debug.LogWarning("No current scene provided. Using the existing currentScene value.");
            }

            // Save the current scene state
            SaveCurrentSceneState();

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
                currentScene = currentScene,
                coinCount = coinCount,
                inventory = inventoryQuantities,
                itemSprites = inventorySprites,
                completedScenes = completedScenes,
                questCompletionStatus = questCompletionStatus,
                sceneStates = sceneStates // Save all scene states
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

    public void LoadGame(string nextScene = null)
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                Debug.Log($"NEXT SCENE!!! : {nextScene}");
                string json = File.ReadAllText(saveFilePath);
                SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);

                // Restore game data
                coinCount = saveData.coinCount;
                completedScenes = saveData.completedScenes ?? new List<string>();
                questCompletionStatus = saveData.questCompletionStatus ?? new Dictionary<string, bool[]>();
                sceneStates = saveData.sceneStates ?? new Dictionary<string, SceneState>();


                Debug.Log($"Loaded completed scenes: {string.Join(", ", completedScenes)}");

                // Update the coin UI
                if (CoinUIManager.Instance != null)
                {
                    CoinUIManager.Instance.UpdateCoinUI(coinCount);
                }

                // Determine the scene to load
                if (!string.IsNullOrEmpty(nextScene))
                {
                    // Load the specified next scene
                    currentScene = nextScene;
                    Debug.Log($"Loading specified next scene: {currentScene}");
                    SceneManager.LoadScene(currentScene);
                }
                else if (!string.IsNullOrEmpty(saveData.currentScene))
                {
                    // Load the saved current scene
                    currentScene = saveData.currentScene;
                    Debug.Log($"Loading saved scene: {currentScene}");
                    SceneManager.LoadScene(currentScene);
                }
                else
                {
                    // Fallback to a default scene
                    Debug.LogWarning("No current scene saved or specified. Loading fallback scene.");
                    currentScene = "Main Menu"; // Replace with your fallback scene name
                    SceneManager.LoadScene(currentScene);
                }

                Debug.Log("Game loaded successfully.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load game: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning("No save file found. Starting a new game.");
            // Fallback to a default scene if no save file exists
            currentScene = "MainMenu"; // Replace with your fallback scene name
            SceneManager.LoadScene(currentScene);
        }
    }

    public void MarkSceneAsCompleted(string sceneName)
    {
        if (!completedScenes.Contains(sceneName))
        {
            completedScenes.Add(sceneName);
            Debug.Log($"Scene '{sceneName}' marked as completed.");
        }
    }

    public bool SaveQuestState(string questName, bool[] taskCompletionStatus)
    {
        if (string.IsNullOrEmpty(questName))
        {
            Debug.LogError("SaveQuestState: questName is null or empty. Cannot save quest state.");
            return false; // Indicate failure
        }

        if (taskCompletionStatus == null)
        {
            Debug.LogError($"SaveQuestState: taskCompletionStatus for quest '{questName}' is null. Cannot save quest state.");
            return false; // Indicate failure
        }

        // Save the task completion status for the given quest
        if (questCompletionStatus.ContainsKey(questName))
        {
            questCompletionStatus[questName] = taskCompletionStatus;
        }
        else
        {
            questCompletionStatus.Add(questName, taskCompletionStatus);
        }

        Debug.Log($"Quest state for '{questName}' saved successfully.");
        return true; // Indicate success
    }

    public bool[] LoadQuestState(string questName, int taskCount)
    {
        if (string.IsNullOrEmpty(questName))
        {
            Debug.LogError("LoadQuestState: questName is null or empty. Cannot load quest state.");
            return new bool[taskCount]; // Return default state
        }

        if (questCompletionStatus.ContainsKey(questName))
        {
            return questCompletionStatus[questName];
        }
        else
        {
            Debug.LogWarning($"LoadQuestState: No saved state found for quest '{questName}'. Initializing default state.");
            return new bool[taskCount]; // Return default state
        }
    }


    // -------------------- For the Shop Manager --------------------
    public void AddCoins(int amount)
    {
        coinCount += amount;
        Debug.Log($"Added {amount} coins. Total coins: {coinCount}");

        // Update the coin UI
        if (CoinUIManager.Instance != null)
        {

            CoinUIManager.Instance.UpdateCoinUI(coinCount);
        }
    }

    public bool SpendCoins(int amount)
    {



        if (coinCount >= amount)
        {
            coinCount -= amount;
            Debug.Log($"Spent {amount} coins. Remaining coins: {coinCount}");

            // Update the coin UI
            if (CoinUIManager.Instance != null)
            {
                CoinUIManager.Instance.UpdateCoinUI(coinCount);
            }
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough coins to complete the purchase!");
            return false;
        }
    }
    /* 
        public void BuyTime(int cost, float timeToAdd)
        {
            if (SpendCoins(cost))
            {
                TaymerManager taymerManager = FindObjectOfType<TaymerManager>();
                if (taymerManager != null)
                {
                    taymerManager.AddTime(timeToAdd);
                    Debug.Log($"{timeToAdd} seconds added to the timer.");
                }
            }
        }

        public void BuyLife(int cost)
        {
            if (SpendCoins(cost))
            {
                LayfManager layfManager = FindObjectOfType<LayfManager>();
                if (layfManager != null)
                {
                    layfManager.AddLife();
                    Debug.Log("1 life added.");
                }
            }
        }

        public void BuyHint(int cost)
        {
            if (SpendCoins(cost))
            {
                TaymerManager taymerManager = FindObjectOfType<TaymerManager>();
                if (taymerManager != null)
                {
                    taymerManager.UseHint();
                    Debug.Log("Hint purchased and used.");
                }
            }
        }

     */

    public void BuyTime(int cost, float timeToAdd)
    {
        /*      if (SpendCoins(cost))
             {
                 TaymerManager taymerManager = FindObjectOfType<TaymerManager>();
                 if (taymerManager != null)
                 {
                     //  taymerManager.AddTime(timeToAdd); // Apply time directly
                     taymerManager.AddAdditionalTime(timeToAdd); // Apply time directly
                     Debug.Log($"{timeToAdd} seconds added to the timer.");
                 }
                 else
                 {
                     // Store the purchased time in GameManager for later use
                     additionalTime += (int)timeToAdd;
                     Debug.Log($"TaymerManager not found! Storing {timeToAdd} seconds for later. Total additional time: {additionalTime}");

                 }
             } */
        /*  if (SpendCoins(cost))
         {
             GameManager.Instance.additionalTime += (int)timeToAdd;
             Debug.Log($"[Shop] Additional time purchased: {timeToAdd}. Total additional time: {GameManager.Instance.additionalTime}");
         } */
        if (GameManager.Instance == null)
        {
            Debug.LogError("[ShopManager] GameManager.Instance is null!");
            return;
        }

        if (SpendCoins(cost))
        {
            GameManager.Instance.additionalTime += (int)timeToAdd;
            Debug.Log($"[ShopManager] Purchased {timeToAdd} seconds. Total additional time: {GameManager.Instance.additionalTime}");
        }
        else
        {
            Debug.LogWarning("[ShopManager] Not enough coins to buy time!");
        }
    }

    public void BuyLife(int cost)
    {
        if (SpendCoins(cost))
        {
            LayfManager layfManager = FindObjectOfType<LayfManager>();
            if (layfManager != null)
            {
                layfManager.AddLife(); // Apply life directly
                Debug.Log("1 life added.");
            }
            else
            {
                // Store the purchased life in GameManager for later use
                additionalLives += 1;
                Debug.Log($"LayfManager not found! Storing 1 life for later. Total additional lives: {additionalLives}");

            }
        }
    }

    public void BuyHint(int cost)
    {
        if (SpendCoins(cost))
        {
            TaymerManager taymerManager = FindObjectOfType<TaymerManager>();
            if (taymerManager != null)
            {
                taymerManager.UseHint(); // Apply hint directly
                Debug.Log("Hint purchased and used.");
            }
            else
            {
                // Store the purchased hint in GameManager for later use
                additionalHints += 1;
                Debug.Log($"TaymerManager not found! Storing 1 hint for later. Total additional hints: {additionalHints}");

            }
        }
    }
    public void SaveObjectState(string objectName, ObjectState state)
    {
        if (objectStates.ContainsKey(objectName))
        {
            objectStates[objectName] = state;
        }
        else
        {
            objectStates.Add(objectName, state);
        }
        Debug.Log($"[GameManager] Saved state for {objectName}: {state.isActive}");
    }

    public ObjectState LoadObjectState(string objectName)
    {
        if (objectStates.TryGetValue(objectName, out ObjectState state))
        {
            Debug.Log($"[GameManager] Loaded state for {objectName}: {state.isActive}");
            return state;
        }
        Debug.Log($"[GameManager] No saved state found for {objectName}.");
        return null;
    }
    // -------------------- Save Data Class --------------------

    [System.Serializable]
    public class SaveData
    {
        public string currentScene;
        public int coinCount;
        public Dictionary<string, int> inventory; // For saving only quantities
        public Dictionary<string, string> itemSprites; // For saving sprite names
        public List<string> completedScenes; // List of completed scenes
        public Dictionary<string, bool[]> questCompletionStatus; // Quest completion status
        public Dictionary<string, SceneState> sceneStates; // Save all scene states
    }

    [System.Serializable]
    public class SceneState
    {
        public bool isCompleted;
        public Dictionary<string, bool[]> questStates = new Dictionary<string, bool[]>(); // Quest states for this scene
    }


    [System.Serializable]
    public class ObjectState
    {
        public bool isActive;
        public Vector3 position;
        public Quaternion rotation;
    }
}