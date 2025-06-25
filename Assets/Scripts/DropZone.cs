using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    private GoBagQuizManager quizManager;
    private TimeManager timeManager;

    void Start()
    {
        // Try to find GoBagQuizManager first
        quizManager = FindFirstObjectByType<GoBagQuizManager>();

        // If not found, try TimeManager (for backward compatibility)
        if (quizManager == null)
        {
            timeManager = TimeManager.Instance;
            if (timeManager == null)
            {
                Debug.LogError("Neither GoBagQuizManager nor TimeManager found in scene!");
            }
            else
            {
                Debug.Log("Using TimeManager for item drops");
            }
        }
        else
        {
            Debug.Log("Using GoBagQuizManager for item drops");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        DragItem draggedItem = eventData.pointerDrag.GetComponent<DragItem>();

        if (draggedItem == null || draggedItem.itemData == null)
        {
            Debug.LogWarning("Dropped object is missing DragItem or ItemData.");
            return;
        }

        bool isCorrect = draggedItem.itemData.isRequired;
        Debug.Log($"Item dropped: {draggedItem.itemData.itemName}, isRequired: {isCorrect}");

        // Use GoBagQuizManager if available
        if (quizManager != null)
        {
            if (isCorrect)
            {
                Debug.Log("Calling OnItemDropped for correct item (GoBagQuizManager)");
                quizManager.OnItemDropped(draggedItem.itemData);
                draggedItem.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Calling OnWrongItemDropped for wrong item (GoBagQuizManager)");
                quizManager.OnWrongItemDropped(draggedItem.itemData);
                draggedItem.gameObject.SetActive(false);
            }
        }
        // Fallback to TimeManager if GoBagQuizManager not available
        else if (timeManager != null)
        {
            Debug.Log("Using TimeManager for item drop");
            timeManager.HandleItemDrop(isCorrect);
            draggedItem.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("No manager available to handle item drop!");
        }

        // Disable dragging after drop
        draggedItem.enabled = false;
    }
}
