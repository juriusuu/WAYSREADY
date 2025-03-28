using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Transform player; // Assign the player transform in the Inspector
    public float speed = 5f; // Speed at which the player moves

    private Vector3 initialPosition; // Initial position of the joystick handle
    private Vector3 direction; // Direction of movement

    void Start()
    {
        initialPosition = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.position = initialPosition;
        direction = Vector3.zero;
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 currentPosition = transform.position;
        direction = (currentPosition - initialPosition).normalized;

        float distanceFromCenter = Vector3.Distance(currentPosition, initialPosition);
        if (distanceFromCenter > 50f) // Maximum distance from center
        {
            direction *= 50f / distanceFromCenter; // Clamp direction vector length
            transform.position = initialPosition + direction * 50f; // Clamp handle position
        }

        MovePlayer();
    }

    private void MovePlayer()
    {
        if (player != null)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * speed;
            }
            else
            {
                player.Translate(direction * speed * Time.deltaTime);
            }
        }
    }
}