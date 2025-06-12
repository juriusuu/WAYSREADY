using UnityEngine;
using System.Collections.Generic;

public class ShowcaseSequenceManager : MonoBehaviour
{
    public ShowcaseCameraController showcaseCameraController;
    public List<Transform> itemFocusPoints; // Assign all 11 focus points in Inspector
    public Transform playerFocusPoint;      // Assign the player focus point in Inspector

    // Call this after your TV panels finish
    public void StartShowcaseSequence()
    {
        showcaseCameraController.ShowItemsSequentially(itemFocusPoints, playerFocusPoint);
    }
}