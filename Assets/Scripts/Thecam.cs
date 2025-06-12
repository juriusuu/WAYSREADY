using UnityEngine;
using System.Collections;

public class SimpleCameraCinematic : MonoBehaviour
{
    public Camera cinematicCam;         // Assign your Camera in Inspector
    public Transform[] objectsToShow;   // Assign your 11 objects in Inspector
    public float focusDuration = 2f;    // Time to focus on each object
    public Vector3 offset = new Vector3(0, 2, -3); // Camera offset from object

    public void ShowAllObjectsCinematic()
    {
        StartCoroutine(CinematicSequence());
    }

    IEnumerator CinematicSequence()
    {
        cinematicCam.enabled = true;

        foreach (Transform obj in objectsToShow)
        {
            cinematicCam.transform.position = obj.position + offset;
            cinematicCam.transform.LookAt(obj);
            yield return new WaitForSeconds(focusDuration);
        }

        cinematicCam.enabled = false; // Optionally disable after cinematic
    }
}