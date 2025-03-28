using System;
using UnityEngine;
using UnityEngine.UI;

public class PickupButton : MonoBehaviour
{
    public Button button; // Reference to the UI Button
    public PickupItem pickupItem; // Public reference to the PickupItem (assign in Inspector)

    private void Start()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
            if (button == null)
            {
                Debug.LogError("Button not found!");
                return;
            }
        }

        button.onClick.AddListener(OnPickupButtonPressed);
        Debug.Log("Button listener added for pickup button.");

        Debug.Log("PickupItem assigned in PickupButton: " + (pickupItem != null ? pickupItem.ItemName : "null"));
    }

    public void OnPickupButtonPressed()
    {
        Debug.Log("Pickup button pressed.");
        if (pickupItem == null)
        {
            Debug.Log("PickupItem reference is not set in PickupButton.");
            return;
        }

        pickupItem.OnPickupButtonPressed();
    }

    public void SetPickupItem(PickupItem pickupItem)
    {
        this.pickupItem = pickupItem;
        Debug.Log("PickupItem assigned in PickupButton: " + (pickupItem != null ? pickupItem.ItemName : "null"));
    }
}