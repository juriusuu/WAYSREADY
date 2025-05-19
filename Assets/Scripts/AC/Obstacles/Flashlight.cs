using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlightLight;

    private MeshRenderer meshRenderer;
    private void Start()
    {    // Cache the mesh renderer for efficiency
        Transform meshTransform = transform.Find("skpE937");
        if (meshTransform != null)
            meshRenderer = meshTransform.GetComponent<MeshRenderer>();

        if (flashlightLight != null)
        {
            flashlightLight.enabled = false; // Start with flashlight OFF
            flashlightLight.intensity = 8f; // Make it brighter
            flashlightLight.range = 30f;    // Make it shine farther
            // flashlightLight.spotAngle = 60f; // Optional: widen the beam for Spot Light
        }
        if (meshRenderer != null)
            meshRenderer.enabled = false;
    }

    public void ToggleFlashlight(bool state)
    {
        if (flashlightLight != null)
            flashlightLight.enabled = state;
        if (meshRenderer != null)
            meshRenderer.enabled = state;
    }
}