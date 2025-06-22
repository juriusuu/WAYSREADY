using UnityEngine;

[CreateAssetMenu(menuName = "GoBag/ItemData")]
public class ItemData : ScriptableObject {
    public string itemName;
    public Sprite icon;
    public bool isRequired;
}
