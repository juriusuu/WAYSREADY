using UnityEngine;
using UnityEngine.UI;
// Uncomment this if you're using TextMeshPro
// using TMPro;

public class GoBagQuizManager : MonoBehaviour {
    public ItemData[] items;
    public Transform dragItemsParent;
    public GameObject dragItemPrefab;

   void Start() {
    foreach (var item in items) {
        GameObject go = Instantiate(dragItemPrefab, dragItemsParent);
        DragItem di = go.GetComponent<DragItem>();
        if (di != null) {
            di.itemData = item;
        }

        // Set icon and name
        Image iconImage = go.GetComponent<Image>();
        if (iconImage != null && item.icon != null) {
            iconImage.sprite = item.icon;
        }

        Text label = go.GetComponentInChildren<Text>();
        if (label != null) {
            label.text = item.itemName;
        }

        // 🎯 Randomize local position within the panel's area
        RectTransform parentRect = dragItemsParent.GetComponent<RectTransform>();
        RectTransform itemRect = go.GetComponent<RectTransform>();

        float panelWidth = parentRect.rect.width;
        float panelHeight = parentRect.rect.height;

        float randomX = Random.Range(0, panelWidth - itemRect.rect.width);
        float randomY = Random.Range(0, panelHeight - itemRect.rect.height);

        itemRect.anchoredPosition = new Vector2(randomX, -randomY);
    }
}

}
