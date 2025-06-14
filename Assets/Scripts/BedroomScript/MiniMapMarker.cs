/* using UnityEngine;

public class MinimapMarker : MonoBehaviour
{
    public Transform target; // The object to track
    public RectTransform minimapRect; // The minimap RawImage RectTransform
    public float mapWorldSize = 50f; // The size (width) of the minimap area in world units

    void Update()
    {
        Vector3 pos = target.position;
        // Convert world position to minimap position (adjust for your setup)
        float normalizedX = pos.x / mapWorldSize;
        float normalizedZ = pos.z / mapWorldSize;
        minimapRect.anchoredPosition = new Vector2(normalizedX * minimapRect.sizeDelta.x, normalizedZ * minimapRect.sizeDelta.y);
    }
} */

/* using UnityEngine;

public class MinimapMarker : MonoBehaviour
{
    public Transform target; // The object to track
    public RectTransform minimapRect; // The minimap RawImage RectTransform

    void Update()
    {
        Vector3 pos = target.position;
        // Set minimap marker position directly (customize as needed)
        minimapRect.anchoredPosition = new Vector2(pos.x, pos.z);
    }
} */

using UnityEngine;

public class MinimapMarker : MonoBehaviour
{
    public Transform target; // The object to track (player)
    public RectTransform minimapRect; // The minimap UI RectTransform
    public Camera minimapCamera; // Reference to your minimap camera

    void Update()
    {
        // Get the visible world area from the minimap camera
        float orthoSize = minimapCamera.orthographicSize;
        float aspect = minimapCamera.aspect;

        // World bounds
        float worldWidth = orthoSize * 2f * aspect;
        float worldHeight = orthoSize * 2f;

        Vector3 camPos = minimapCamera.transform.position;

        // Normalize player position within the minimap camera's view
        float normalizedX = (target.position.x - (camPos.x - worldWidth / 2f)) / worldWidth;
        float normalizedY = (target.position.z - (camPos.z - worldHeight / 2f)) / worldHeight;

        // Convert to minimap UI position
        float markerX = (normalizedX - 0.5f) * minimapRect.sizeDelta.x;
        float markerY = (normalizedY - 0.5f) * minimapRect.sizeDelta.y;

        ((RectTransform)transform).anchoredPosition = new Vector2(markerX, markerY);
    }
}