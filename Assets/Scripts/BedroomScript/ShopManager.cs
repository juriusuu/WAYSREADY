using UnityEngine;

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
}