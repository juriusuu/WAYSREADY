using UnityEngine;

public class PhoneUseZone : MonoBehaviour
{
    public static bool PlayerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            PlayerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            PlayerInside = false;
    }
}