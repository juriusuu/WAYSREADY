using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObjects/StageData", order = 2)]
public class StageDataSO : ScriptableObject
{
    [Header("Stage Settings")]
    public string stageName; // Name of the stage
    public int initialLives = 3; // Number of lives for this stage
    public float totalTime = 60f; // Total time for this stage

    [Header("Hints")]
    public int maxHints = 3; // Maximum number of hints available for this stage
}