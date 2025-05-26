using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public string keyName; // Set this in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerKeyHolder keyHolder = other.GetComponent<PlayerKeyHolder>();
            if (keyHolder != null)
            {
                keyHolder.AddKey(keyName);
                Destroy(gameObject); // Remove the key from the scene
            }
        }
    }
}