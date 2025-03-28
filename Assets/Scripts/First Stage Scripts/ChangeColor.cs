using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    // The color you want to change to
    private Color targetColor = new Color(61f / 255f, 146f / 255f, 199f / 255f);

    void Start()
    {
        // Get the Renderer component
        Renderer renderer = GetComponent<Renderer>();

        // Check if the Renderer is found
        if (renderer != null)
        {
            // Change the material color
            renderer.material.color = targetColor;
        }
        else
        {
            Debug.LogError("Renderer not found on this object.");
        }
    }
}