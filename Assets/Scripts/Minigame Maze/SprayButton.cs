using UnityEngine;
using UnityEngine.UI;

public class SprayButton : MonoBehaviour
{
    public FireExtinguisher fireExtinguisher; // Reference to the FireExtinguisher script
    public float extinguishAmount = 1f; // Amount to extinguish per button press
    public float destroyDelay = 2f; // Delay before destroying the fire object

    private Button button;
    private Fire fireScript; // Reference to the Fire script on the fire object

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnSprayButtonPressed);
        button.interactable = false; // Disable the button initially
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered with: " + other.gameObject.name); // Log all objects
        if (other.CompareTag("Fire")) // Check if the object has the "Fire" tag
        {
            Debug.Log("Entered trigger with fire object");
            button.interactable = true; // Enable the button
            fireScript = other.GetComponent<Fire>(); // Get the Fire script from the fire object
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exited with: " + other.gameObject.name); // Log all objects
        if (other.CompareTag("Fire")) // Check if the object has the "Fire" tag
        {
            Debug.Log("Exited trigger with fire object");
            button.interactable = false; // Disable the button
            fireScript = null; // Clear the Fire script reference
        }
    }

    private void OnSprayButtonPressed()
    {
        if (fireExtinguisher != null && fireScript != null)
        {
            fireExtinguisher.UseExtinguisher(); // Activate the extinguisher particle system
            fireScript.Extinguish(extinguishAmount); // Call the Extinguish method on the fire

            if (fireScript.ExtinguishProgress >= fireScript.extinguishTime)
            {
                Destroy(fireScript.gameObject, destroyDelay); // Destroy the fire object after a delay
            }
        }
    }
}