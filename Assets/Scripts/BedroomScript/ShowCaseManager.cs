using UnityEngine;
using System.Collections.Generic;

public class ShowcaseSequenceManager : MonoBehaviour
{
    public ShowcaseCameraController showcaseCameraController;
    public List<Transform> itemFocusPoints;
    public Transform playerFocusPoint;
    public TVNextPanelManager tvNextPanelManager;

    void Start()
    {
        if (tvNextPanelManager != null)
        {
            tvNextPanelManager.OnPanelsFinished += StartShowcaseSequence;
        }
    }

    public void StartShowcaseSequence()
    {
        Supercyan.FreeSample.Joystickscript.IsShowcasing = true; // Pause movement
        StartCoroutine(ShowcaseCutsceneCoroutine());
    }

    private System.Collections.IEnumerator ShowcaseCutsceneCoroutine()
    {
        yield return StartCoroutine(showcaseCameraController.ShowItemsSequenceCoroutine(itemFocusPoints, playerFocusPoint));
        Supercyan.FreeSample.Joystickscript.IsShowcasing = false; // Resume movement
    }
}