/* using UnityEngine;

public class StageDataAssigner : MonoBehaviour
{
    public StageDataSO stageData; // Assign the correct StageDataSO in the Inspector

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentStageData = stageData; // Assign the StageDataSO
            Debug.Log($"Assigned {stageData.stageName} to SolidGameManager.");
        }
        else
        {
            Debug.LogError("SolidGameManager instance not found!");
        }
    }
} */