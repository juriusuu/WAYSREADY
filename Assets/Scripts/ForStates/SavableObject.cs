using UnityEngine;

public class SavableObject : MonoBehaviour
{
    private string uniqueID;

    private void Awake()
    {
        // Generate a unique ID for this object
        uniqueID = gameObject.name + "_" + transform.GetInstanceID();
    }

    public void SaveState()
    {
        // Save the object's position, rotation, and active state
        var state = new ObjectState
        {
            position = transform.position,
            rotation = transform.rotation,
            isActive = gameObject.activeSelf
        };

        GameStateManager.Instance.SaveObjectState(uniqueID, state);
    }

    public void RestoreState()
    {
        // Load the object's state
        var state = GameStateManager.Instance.LoadObjectState(uniqueID);
        if (state != null)
        {
            transform.position = state.position;
            transform.rotation = state.rotation;
            gameObject.SetActive(state.isActive);
        }
    }
}