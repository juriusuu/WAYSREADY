/* using UnityEngine;

public class HouseEntrance : MonoBehaviour
{
    public string requiredKeyName; // Set this in the Inspector

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerKeyHolder keyHolder = collision.gameObject.GetComponent<PlayerKeyHolder>();
            if (keyHolder != null && keyHolder.HasKey(requiredKeyName))
            {
                Debug.Log("You have the correct key! Entering the house...");
                // Disable the collider to allow entry
                GetComponent<Collider>().enabled = false;
            }
            else
            {
                Debug.Log("You need the correct key to enter!");
            }
        }
    }
} */


using UnityEngine;
using System.Collections;

public class HouseEntrance : MonoBehaviour
{
    public string requiredKeyName; // Set this in the Inspector
    public GameObject needKeyPanel; // Assign your UI panel in the Inspector

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerKeyHolder keyHolder = collision.gameObject.GetComponent<PlayerKeyHolder>();
            if (keyHolder != null && keyHolder.HasKey(requiredKeyName))
            {
                Debug.Log("You have the correct key! Entering the house...");
                // Deactivate the whole GameObject to remove the barrier
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("You need the correct key to enter!");
                if (needKeyPanel != null)
                    StartCoroutine(ShowPanelForSeconds(1.5f));
            }
        }
    }

    private IEnumerator ShowPanelForSeconds(float seconds)
    {
        needKeyPanel.SetActive(true);
        yield return new WaitForSeconds(seconds);
        needKeyPanel.SetActive(false);
    }
}