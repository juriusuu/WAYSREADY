using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameDataSO : ScriptableObject
{
    [Header("Current Stage Data")]
    public StageDataSO currentStage; // Reference to the current stage's data

    [Header("Runtime Data")]
    public int currentLives; // Lives during gameplay
    public float remainingTime; // Remaining time during gameplay

    public void InitializeStageData()
    {
        if (currentStage != null)
        {
            currentLives = currentStage.initialLives;
            remainingTime = currentStage.totalTime;
        }
        else
        {
            Debug.LogWarning("No current stage assigned to GameDataSO!");
        }
    }
}