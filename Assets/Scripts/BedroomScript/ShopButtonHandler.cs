using UnityEngine;

public class ShopButtonHandler : MonoBehaviour
{
    public void BuyTime()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.BuyTime(20, 10f); // Example: Cost = 20, Time = 10 seconds
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    public void BuyLife()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.BuyLife(30); // Example: Cost = 30
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    public void BuyHint()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.BuyHint(30); // Example: Cost = 30
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }
}