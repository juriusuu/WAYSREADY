using UnityEngine;

public class TreeTrunkCarry : MonoBehaviour
{
    public Transform carryPoint; // Assign in Inspector (the CarryPoint you created)
    private GameObject carriedTrunk = null;

    [SerializeField] private Animator playerAnimator;

    // Call this from your UI button's OnClick event
    public void OnPickupDropButtonPressed()
    {
        if (carriedTrunk == null)
            TryPickup();
        else
            Drop();
    }

    void TryPickup()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Center of screen

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10f))
        {
            if (hit.collider.CompareTag("TreeTrunk"))
            {
                PickupTrunk(hit.collider.gameObject);
            }
        }
    }

    // Call this from a trigger event on the trunk
    public void TryPickupWithTrigger(GameObject trunk)
    {
        if (carriedTrunk == null && trunk.CompareTag("TreeTrunk"))
        {
            PickupTrunk(trunk);
        }
    }

    void PickupTrunk(GameObject trunk)
    {
        carriedTrunk = trunk;
        carriedTrunk.transform.SetParent(carryPoint);
        carriedTrunk.transform.localPosition = Vector3.zero;
        carriedTrunk.GetComponent<Rigidbody>().isKinematic = true;
        // Disable collider while carrying
        carriedTrunk.GetComponent<Collider>().enabled = false;

        // Trigger pickup animation
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Pickup");
        }
    }

    void Drop()
    {
        carriedTrunk.transform.SetParent(null);
        carriedTrunk.GetComponent<Rigidbody>().isKinematic = false;

        // Offset the trunk forward from the player to avoid overlap
        carriedTrunk.transform.position += transform.forward * 1.0f;
        carriedTrunk.GetComponent<Collider>().enabled = true;

        // Trigger drop-off animation
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Drop");
        }

        carriedTrunk = null;
    }
}