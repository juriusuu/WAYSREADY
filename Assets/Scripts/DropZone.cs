using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public TimeManager timeManager;

    public void OnDrop(PointerEventData eventData)
    {
        DragItem draggedItem = eventData.pointerDrag?.GetComponent<DragItem>();

        if (draggedItem != null && draggedItem.itemData != null)
        {
            bool isCorrect = draggedItem.itemData.isRequired;

            // Snap item to drop zone
            draggedItem.transform.SetParent(transform);
            draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            // Inform GameManager
            TimeManager.Instance.HandleItemDrop(draggedItem.itemData);
        }
        else
        {
            Debug.LogWarning("Dropped item is not valid or missing itemData.");
        }
    }
}
