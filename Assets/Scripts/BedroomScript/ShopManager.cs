using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int timeCost = 20; // Cost of adding time
    public int lifeCost = 30; // Cost of adding a life
    public int hintCost = 30; // Cost of adding a hint

    public Button timeButton;  // Assign in the Inspector
    public Button lifeButton;  // Assign in the Inspector
    public Button hintButton;  // Assign in the Inspector
    public static ShopManager Instance; // Singleton instance

    private void Awake()
    {
        /*         if (FindObjectsOfType<ShopManager>().Length > 1)
                {
                    Destroy(gameObject); // Prevent duplicates
                    return;
                }

                DontDestroyOnLoad(gameObject); // Persist this GameObject across scenes */

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }

    }

    public void ReassignOnClickEvents()
    {
        if (timeButton != null)
        {
            timeButton.onClick.RemoveAllListeners();
            timeButton.onClick.AddListener(() => BuyTime()); // Example: Adjust as needed
        }

        if (lifeButton != null)
        {
            lifeButton.onClick.RemoveAllListeners();
            lifeButton.onClick.AddListener(() => BuyLife()); // Example: Adjust as needed
        }

        if (hintButton != null)
        {
            hintButton.onClick.RemoveAllListeners();
            hintButton.onClick.AddListener(() => BuyHint()); // Example: Adjust as needed
        }

        Debug.Log("OnClick events reassigned for Shop buttons.");
    }
    public void ReassignButtonEvents()
    {
        // Check if the ShopMenu is assigned
        if (shopMenu == null)
        {
            Debug.LogError("ShopMenu is null. Cannot reassign button references.");
            return;
        }

        // Dynamically find the buttons if their references are missing
        if (timeButton == null)
        {
            timeButton = shopMenu.transform.Find("CrystalButton")?.GetComponent<Button>();
            if (timeButton == null)
            {
                Debug.LogError("TimeButton not found in ShopMenu!");
            }
        }

        if (lifeButton == null)
        {
            lifeButton = shopMenu.transform.Find("HeartButton")?.GetComponent<Button>();
            if (lifeButton == null)
            {
                Debug.LogError("LifeButton not found in ShopMenu!");
            }
        }

        if (hintButton == null)
        {
            hintButton = shopMenu.transform.Find("HintButton")?.GetComponent<Button>();
            if (hintButton == null)
            {
                Debug.LogError("HintButton not found in ShopMenu!");
            }
        }

        // Find the Shop button in the Main Menu
        Button shopButton = GameObject.Find("Shop")?.GetComponent<Button>();
        if (shopButton != null)
        {
            shopButton.onClick.RemoveAllListeners();
            shopButton.onClick.AddListener(OpenShop); // Assign the OpenShop method
            Debug.Log("Reassigned OnClick event for Shop button.");
        }
        else
        {
            Debug.LogError("Shop button not found in the scene!");
        }

        // Reassign OnClick events for the buttons
        if (timeButton != null)
        {
            timeButton.onClick.RemoveAllListeners();
            timeButton.onClick.AddListener(() => BuyTime());
            Debug.Log("Reassigned OnClick event for Time button.");
        }

        if (lifeButton != null)
        {
            lifeButton.onClick.RemoveAllListeners();
            lifeButton.onClick.AddListener(() => BuyLife());
            Debug.Log("Reassigned OnClick event for Life button.");
        }

        if (hintButton != null)
        {
            hintButton.onClick.RemoveAllListeners();
            hintButton.onClick.AddListener(() => BuyHint());
            Debug.Log("Reassigned OnClick event for Hint button.");
        }
    }
    /* 
    public void ReassignButtonEvents()
    {
        if (timeButton != null)
        {
            timeButton.onClick.RemoveAllListeners();
            timeButton.onClick.AddListener(() => BuyTime());
            Debug.Log("Reassigned OnClick event for Time button.");
        }
        else
        {
            Debug.LogError("Time button reference is missing!");
        }

        if (lifeButton != null)
        {
            lifeButton.onClick.RemoveAllListeners();
            lifeButton.onClick.AddListener(() => BuyLife());
            Debug.Log("Reassigned OnClick event for Life button.");
        }
        else
        {
            Debug.LogError("Life button reference is missing!");
        }

        if (hintButton != null)
        {
            hintButton.onClick.RemoveAllListeners();
            hintButton.onClick.AddListener(() => BuyHint());
            Debug.Log("Reassigned OnClick event for Hint button.");
        }
        else
        {
            Debug.LogError("Hint button reference is missing!");
        }
    }
 */
    /* 
        public void ReassignButtonEvents()
        {
            // Find the button in the scene
            Button timeButton = GameObject.Find("CrystalButton").GetComponent<Button>();
            if (timeButton != null)
            {
                timeButton.onClick.RemoveAllListeners(); // Clear existing listeners
                timeButton.onClick.AddListener(() => BuyTime()); // Reassign the BuyTime method
                Debug.Log("Reassigned OnClick event for Time button.");
            }
            else
            {
                Debug.LogError("Time button not found in the scene!");
            }

            // Find the button in the scene
            Button lifeButton = GameObject.Find("HeartButton").GetComponent<Button>();
            if (timeButton != null)
            {
                lifeButton.onClick.RemoveAllListeners(); // Clear existing listeners
                lifeButton.onClick.AddListener(() => BuyLife()); // Reassign the BuyTime method
                Debug.Log("Reassigned OnClick event for Time button.");
            }
            else
            {
                Debug.LogError("Time button not found in the scene!");
            }

            // Find the button in the scene
            Button hintButton = GameObject.Find("HintButton").GetComponent<Button>();
            if (timeButton != null)
            {
                hintButton.onClick.RemoveAllListeners(); // Clear existing listeners
                hintButton.onClick.AddListener(() => BuyHint()); // Reassign the BuyTime method
                Debug.Log("Reassigned OnClick event for Time button.");
            }
            else
            {
                Debug.LogError("Time button not found in the scene!");
            }
        }
     */


    public void BuyTime()
    {
        Debug.Log("[ShopManager] BuyTime method called.");
        if (GameManager.Instance != null && GameManager.Instance.SpendCoins(timeCost))
        {
            GameManager.Instance.additionalTime += 10; // Add 10 seconds to be applied later
            Debug.Log("Time purchased! Additional time: " + GameManager.Instance.additionalTime);

            // Apply time directly if TaymerManager is present in the scene
            TaymerManager taymerManager = FindObjectOfType<TaymerManager>();
            if (taymerManager != null)
            {
                taymerManager.AddTime(10); // Add 10 seconds
                Debug.Log("10 seconds added to the timer in TaymerManager.");
            }
        }
        else
        {
            Debug.LogWarning("Not enough coins to buy time!");
        }
    }

    public void BuyLife()
    {
        if (GameManager.Instance != null && GameManager.Instance.SpendCoins(lifeCost))
        {
            GameManager.Instance.additionalLives += 1; // Add 1 life to be applied later
            Debug.Log("Life purchased! Additional Life: " + GameManager.Instance.additionalLives);
            // Find the LayfManager in the current scene
            LayfManager layfManager = FindObjectOfType<LayfManager>();
            if (layfManager != null)
            {
                layfManager.AddLife(); // Add a life directly to the LayfManager
                Debug.Log("Life purchased and added to LayfManager.");
            }
            else
            {
                // Store the purchased life in GameManager for later use
                //  GameManager.Instance.additionalLives += 1;
                Debug.Log("LayfManager not found! Storing purchased life for later. Total additional lives: " + GameManager.Instance.additionalLives);
            }
        }
        else
        {
            Debug.LogWarning("Not enough coins to buy a life!");
        }
    }

    public GameObject shopMenu; // Assign in the Inspector or initialize in code
    /* 
        public void OpenShop()
        {
            if (shopMenu == null)
            {
                Debug.LogError("ShopMenu is not assigned in the Inspector!");
                return;
            }

            shopMenu.SetActive(true); // Activate the ShopMenu
            Debug.Log("ShopMenu activated.");
        } */

    /* public void OpenShop()
    {
        if (shopMenu == null)
        {
            // Find the ShopMenu dynamically under the Shop GameObject
            GameObject shop = GameObject.Find("Shop"); // Replace "Shop" with the exact name of the parent GameObject
            if (shop != null)
            {
                Transform shopMenuTransform = shop.transform.Find("ShopMenu");
                if (shopMenuTransform != null)
                {
                    shopMenu = shopMenuTransform.gameObject;
                    Debug.Log("ShopMenu reference assigned dynamically.");
                }
                else
                {
                    Debug.LogError("ShopMenu not found under Shop!");
                    return;
                }
            }
            else
            {
                Debug.LogError("Shop GameObject not found in the scene!");
                return;
            }
        }

        // Activate the ShopMenu
        shopMenu.SetActive(true);
        Debug.Log("ShopMenu opened.");
    }
 */
    public void OpenShop()
    {
        if (shopMenu == null)
        {
            Debug.LogError("ShopMenu is not assigned in the Inspector!");
            return;
        }

        shopMenu.SetActive(true); // Activate the ShopMenu
        Debug.Log("ShopMenu opened.");
    }
    public void CloseShop()
    {
        if (shopMenu == null)
        {
            Debug.LogError("ShopMenu is not assigned in the Inspector!");
            return;
        }

        shopMenu.SetActive(false); // Deactivate the ShopMenu
        Debug.Log("ShopMenu deactivated.");
    }

    /*     public void BuyHint()
        {
            if (GameManager.Instance != null && GameManager.Instance.SpendCoins(hintCost))
            {
                GameManager.Instance.additionalHints += 1; // Add 1 hint to be applied later
                Debug.Log("Hint purchased! Additional hints: " + GameManager.Instance.additionalHints);

                // Apply hint directly if TaymerManager is present in the scene
                TaymerManager taymerManager = FindObjectOfType<TaymerManager>();
                if (taymerManager != null)
                {
                    taymerManager.UseHint(); // Use a hint
                    Debug.Log("Hint used in TaymerManager.");
                }
            }
            else
            {
                Debug.LogWarning("Not enough coins to buy a hint!");
            }
        }
    } */

    public void BuyHint()
    {
        if (GameManager.Instance != null && GameManager.Instance.SpendCoins(hintCost))
        {
            GameManager.Instance.additionalHints += 1; // Add 1 hint to be applied later
            Debug.Log($"Hint purchased! Total additional hints: {GameManager.Instance.additionalHints}");

            // Apply hint directly if TaymerManager is present in the scene
            TaymerManager taymerManager = FindObjectOfType<TaymerManager>();
            if (taymerManager != null)
            {
                taymerManager.AddHint(); // Use a hint
                Debug.Log("Hint applied directly in TaymerManager.");
            }
            else
            {
                Debug.Log("TaymerManager not found. Hint will be applied in the next scene.");
            }
        }
        else
        {
            Debug.LogWarning("Not enough coins to buy a hint!");
        }
    }
}

/* using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public int timeCost = 20; // Cost of adding time
    public int lifeCost = 30; // Cost of adding a life
    public int hintCost = 30; // Cost of adding a hint

    public void BuyTime()
    {
        if (GameManager.Instance != null && GameManager.Instance.SpendCoins(timeCost))
        {
            GameManager.Instance.additionalTime += 10; // Add 10 seconds to be applied later
            Debug.Log("Time purchased! Additional time: " + GameManager.Instance.additionalTime);
        }
        else
        {
            Debug.LogWarning("Not enough coins to buy time!");
        }
    }

    public void BuyLife()
    {
        if (GameManager.Instance != null && GameManager.Instance.SpendCoins(lifeCost))
        {
            GameManager.Instance.additionalLives += 1; // Add 1 life to be applied later
            Debug.Log("Life purchased! Additional lives: " + GameManager.Instance.additionalLives);
        }
        else
        {
            Debug.LogWarning("Not enough coins to buy a life!");
        }
    }

    public void BuyHint()
    {
        if (GameManager.Instance != null && GameManager.Instance.SpendCoins(hintCost))
        {
            GameManager.Instance.additionalHints += 1; // Add 1 hint to be applied later
            Debug.Log("Hint purchased! Additional hints: " + GameManager.Instance.additionalHints);
        }
        else
        {
            Debug.LogWarning("Not enough coins to buy a hint!");
        }
    }
} */

/* using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public int timeCost = 20; // Cost of adding time
    public int lifeCost = 30; // Cost of adding a life
    public int hintCost = 30; // Cost of adding a hint

    public void BuyTime()
    {
        if (GameManager.Instance != null && GameManager.Instance.coinCount >= timeCost)
        {
            GameManager.Instance.coinCount -= timeCost; // Deduct points
            Debug.Log("Time purchased! Remaining coins: " + GameManager.Instance.coinCount);

            // Add time to the timer
            TaymerManager taymerManager = FindObjectOfType<TaymerManager>();
            if (taymerManager != null)
            {
                taymerManager.AddTime(10); // Add 10 seconds (or any value you want)
                Debug.Log("10 seconds added to the timer.");
            }
        }
        else
        {
            Debug.LogWarning("Not enough coins to buy time!");
        }
    }

    public void BuyLife()
    {
        if (GameManager.Instance != null && GameManager.Instance.coinCount >= lifeCost)
        {
            GameManager.Instance.coinCount -= lifeCost; // Deduct points
            Debug.Log("Life purchased! Remaining coins: " + GameManager.Instance.coinCount);

            // Add a life
            LayfManager layfManager = FindObjectOfType<LayfManager>();
            if (layfManager != null)
            {
                layfManager.AddLife(); // Add one life
                Debug.Log("1 life added.");
            }
        }
        else
        {
            Debug.LogWarning("Not enough coins to buy a life!");
        }
    }

    public void BuyHint()
    {
        if (GameManager.Instance != null && GameManager.Instance.coinCount >= hintCost)
        {
            GameManager.Instance.coinCount -= hintCost; // Deduct points
            Debug.Log("Hint purchased! Remaining coins: " + GameManager.Instance.coinCount);

            // Add a hint
            TaymerManager taymerManager = FindObjectOfType<TaymerManager>();
            if (taymerManager != null)
            {
                taymerManager.UseHint(); // Add a hint
                Debug.Log("Hint added.");
            }
        }
        else
        {
            Debug.LogWarning("Not enough coins to buy a hint!");
        }
    }
} */