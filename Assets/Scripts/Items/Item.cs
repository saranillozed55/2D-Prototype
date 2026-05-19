using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Shop/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int price;
    public Sprite icon;
}
