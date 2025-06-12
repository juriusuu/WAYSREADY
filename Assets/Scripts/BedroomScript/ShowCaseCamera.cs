using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowcaseCameraController : MonoBehaviour
{
    public Camera showcaseCamera;
    public float showDuration = 1.5f;
    public float moveDuration = 1.0f;

    // Show a single item (existing)
    public void ShowItem(Transform itemFocus, Transform playerFocus)
    {
        StartCoroutine(ShowItemAndMoveToPlayer(itemFocus, playerFocus));
    }

    // Show multiple items in sequence
    public void ShowItemsSequentially(List<Transform> itemFocusPoints, Transform playerFocus)
    {
        StartCoroutine(ShowItemsSequenceCoroutine(itemFocusPoints, playerFocus));
    }

    private IEnumerator ShowItemsSequenceCoroutine(List<Transform> itemFocusPoints, Transform playerFocus)
    {
        foreach (var itemFocus in itemFocusPoints)
        {
            yield return ShowItemAndMoveToPlayer(itemFocus, playerFocus);
            yield return new WaitForSeconds(0.5f); // Optional pause between items
        }
    }

    private IEnumerator ShowItemAndMoveToPlayer(Transform itemFocus, Transform playerFocus)
    {
        showcaseCamera.enabled = true;
        showcaseCamera.transform.position = itemFocus.position;
        showcaseCamera.transform.rotation = itemFocus.rotation;

        yield return new WaitForSeconds(showDuration);

        float elapsed = 0f;
        Vector3 startPos = showcaseCamera.transform.position;
        Quaternion startRot = showcaseCamera.transform.rotation;

        while (elapsed < moveDuration)
        {
            showcaseCamera.transform.position = Vector3.Lerp(startPos, playerFocus.position, elapsed / moveDuration);
            showcaseCamera.transform.rotation = Quaternion.Slerp(startRot, playerFocus.rotation, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        showcaseCamera.transform.position = playerFocus.position;
        showcaseCamera.transform.rotation = playerFocus.rotation;

        yield return new WaitForSeconds(0.5f);

        showcaseCamera.enabled = false;
    }
}