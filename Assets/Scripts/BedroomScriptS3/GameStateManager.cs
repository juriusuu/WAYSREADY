using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    private string saveFilePath;

    private Dictionary<string, ObjectState> sceneState = new Dictionary<string, ObjectState>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
            saveFilePath = Path.Combine(Application.persistentDataPath, "sceneState.json");
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    // Save the scene state to a JSON file
    public void SaveSceneState()
    {
        string json = JsonUtility.ToJson(new SceneStateWrapper(sceneState));
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Scene state saved to {saveFilePath}");
    }

    // Load the scene state from a JSON file
    public void LoadSceneState()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SceneStateWrapper wrapper = JsonUtility.FromJson<SceneStateWrapper>(json);
            sceneState = wrapper.ToDictionary();
            Debug.Log("Scene state loaded.");
        }
        else
        {
            Debug.LogWarning("No saved scene state found.");
        }
    }

    // Save the state of an object
    public void SaveObjectState(string key, ObjectState state)
    {
        if (sceneState.ContainsKey(key))
        {
            sceneState[key] = state;
        }
        else
        {
            sceneState.Add(key, state);
        }
    }

    // Load the state of an object
    public ObjectState LoadObjectState(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("LoadObjectState was called with a null or empty key. Returning default state.");
            return new ObjectState
            {
                position = Vector3.zero,
                rotation = Quaternion.identity,
                isActive = true,
                customData = null // Default to null or empty string
            };
        }

        if (sceneState.ContainsKey(key))
        {
            return sceneState[key];
        }

        Debug.LogWarning($"No saved state found for key '{key}'. Returning default state.");
        return new ObjectState
        {
            position = Vector3.zero,
            rotation = Quaternion.identity,
            isActive = true,
            customData = null // Default to null or empty string
        };
    }
}

// Wrapper class for serializing the scene state dictionary
[System.Serializable]
public class SceneStateWrapper
{
    public List<ObjectStateEntry> entries = new List<ObjectStateEntry>();

    public SceneStateWrapper(Dictionary<string, ObjectState> sceneState)
    {
        foreach (var kvp in sceneState)
        {
            entries.Add(new ObjectStateEntry { key = kvp.Key, state = kvp.Value });
        }
    }

    public Dictionary<string, ObjectState> ToDictionary()
    {
        var dict = new Dictionary<string, ObjectState>();
        foreach (var entry in entries)
        {
            dict[entry.key] = entry.state;
        }
        return dict;
    }
}

[System.Serializable]
public class ObjectStateEntry
{
    public string key;
    public ObjectState state;
}

[System.Serializable]
public class ObjectState
{
    public Vector3 position;
    public Quaternion rotation;
    public bool isActive;
    public string customData;
}