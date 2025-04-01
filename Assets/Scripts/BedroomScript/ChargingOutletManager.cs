using UnityEngine;
using UnityEngine.UI;

public class ChargingOutletManager : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject chargingPanel; // Reference to the "charging" panel
    public GameObject fullyChargedPanel; // Reference to the "fully charged" panel
    public Text chargingStatusText; // Reference to the text inside the charging panel

    private bool isPlayerNearby = false; // Tracks if the player is near the charging outlet
    private InventoryManagers inventoryManager; // Reference to the inventory system

    private void Start()
    {
        // Ensure the button is hidden at the start
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteractButtonPressed); // Add listener for button click
        }

        // Ensure the panels are hidden at the start
        if (chargingPanel != null)
        {
            chargingPanel.SetActive(false);
        }
        if (fullyChargedPanel != null)
        {
            fullyChargedPanel.SetActive(false);
        }

        // Find the inventory manager in the scene
        inventoryManager = FindObjectOfType<InventoryManagers>();
    }

    private void Update()
    {
        // Show or hide the interact button based on proximity
        if (isPlayerNearby)
        {
            interactButton.gameObject.SetActive(true); // Show the button when the player is near
        }
        else
        {
            interactButton.gameObject.SetActive(false); // Hide the button when the player is far
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true; // Set the flag to true when the player enters the trigger
            Debug.Log("Player is near the charging outlet.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Player left the charging outlet.");
        }
    }

    private void OnInteractButtonPressed()
    {
        if (inventoryManager != null)
        {
            // Check if the phone and power bank are in the inventory
            int phoneCount = inventoryManager.GetItemCount("phone"); // Ensure correct case
            int powerBankCount = inventoryManager.GetItemCount("powerbank"); // Ensure correct case

            if (phoneCount <= 0 || powerBankCount <= 0)
            {
                Debug.LogWarning("You need to pick up the phone and power bank before charging!");
                ShowChargingPanel("You need to pick up the phone and power bank before charging!");
                return;
            }

            // Show the charging panel with a success message
            ShowChargingPanel("Phone and PowerBank are now charging!");

            // Start the charging process
            StartCoroutine(HandleChargingPanels());
        }
        else
        {
            Debug.LogError("InventoryManager is not set!");
        }
    }

    private void ShowChargingPanel(string message)
    {
        if (chargingPanel != null && chargingStatusText != null)
        {
            chargingPanel.SetActive(true);
            chargingStatusText.text = message;

            // Hide the panel after 2 seconds
            StartCoroutine(HideChargingPanelAfterDelay(2f));
        }
    }

    private System.Collections.IEnumerator HideChargingPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (chargingPanel != null)
        {
            chargingPanel.SetActive(false);
        }
    }

    private System.Collections.IEnumerator HandleChargingPanels()
    {
        // Show the "charging" panel
        if (chargingPanel != null)
        {
            chargingPanel.SetActive(true);
        }

        if (fullyChargedPanel != null)
        {
            fullyChargedPanel.SetActive(false);
        }

        // Wait for 4 seconds
        yield return new WaitForSeconds(4f);

        // Hide the "charging" panel and show the "fully charged" panel
        if (chargingPanel != null)
        {
            chargingPanel.SetActive(false);
        }

        if (fullyChargedPanel != null)
        {
            fullyChargedPanel.SetActive(true);
        }

        Debug.Log("Charging complete! Phone and PowerBank are fully charged.");

        // Wait for 4 seconds before hiding the "fully charged" panel
        yield return new WaitForSeconds(4f);

        if (fullyChargedPanel != null)
        {
            fullyChargedPanel.SetActive(false);
        }
    }
}
/* using UnityEngine;
using UnityEngine.UI;

public class ChargingOutletManager : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject chargingPanel; // Reference to the charging panel UI
    public Text chargingStatusText; // Reference to the text inside the charging panel

    private bool isPlayerNearby = false; // Tracks if the player is near the charging outlet
    private InventoryManagers inventoryManager; // Reference to the inventory system

    private void Start()
    {
        // Ensure the button is hidden at the start
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteractButtonPressed); // Add listener for button click
        }

        // Ensure the charging panel is hidden at the start
        if (chargingPanel != null)
        {
            chargingPanel.SetActive(false);
        }

        // Find the inventory manager in the scene
        inventoryManager = FindObjectOfType<InventoryManagers>();
    }

    private void Update()
    {
        // Show or hide the interact button based on proximity
        if (isPlayerNearby)
        {
            interactButton.gameObject.SetActive(true); // Show the button when the player is near
        }
        else
        {
            interactButton.gameObject.SetActive(false); // Hide the button when the player is far
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true; // Set the flag to true when the player enters the trigger
            Debug.Log("Player is near the charging outlet.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Player left the charging outlet.");
        }
    }
    private void OnInteractButtonPressed()
    {
        if (inventoryManager != null)
        {
            // Check if the phone and power bank are in the inventory
            int phoneCount = inventoryManager.GetItemCount("phone");
            int powerBankCount = inventoryManager.GetItemCount("powerbank");

            if (phoneCount <= 0 || powerBankCount <= 0)
            {
                Debug.LogWarning("You need to pick up the phone and power bank before charging!");
                ShowChargingPanel("You need to pick up the phone and power bank before charging!");
                return;
            }

            // Show the charging panel with a success message
            ShowChargingPanel("Phone and PowerBank are now charging!");
        }
        else
        {
            Debug.LogError("InventoryManager is not set!");
        }
    }

    private void ShowChargingPanel(string message)
    {
        if (chargingPanel != null && chargingStatusText != null)
        {
            chargingPanel.SetActive(true);
            chargingStatusText.text = message;

            // Hide the panel after 2 seconds
            StartCoroutine(HideChargingPanelAfterDelay(2f));
        }
    }

    private System.Collections.IEnumerator HideChargingPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (chargingPanel != null)
        {
            chargingPanel.SetActive(false);
        }
    }
} */