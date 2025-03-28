using UnityEngine;

public class SandBagDropOff : MonoBehaviour
{
    public SandBagManager sandBagManager; // Reference to the SandBagManager
    public string sandbagType = "Default"; // The type of sandbag to drop

    void Update()
    {
        // Check for input to drop a sandbag (e.g., pressing the "Drop" key)
        if (Input.GetButtonDown("Drop")) // Make sure to set up the "Drop" input in Unity's Input settings
        {
            DropSandbag();
        }
    }

    private void DropSandbag()
    {
        if (sandBagManager != null)
        {
            sandBagManager.TryDropSandbag(sandbagType);
        }
        else
        {
            Debug.Log("SandBagManager reference is not set in SandBagDropOff.");
        }
    }
}