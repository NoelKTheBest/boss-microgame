using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public int itemPositionIndex;
    public string itemName;
    public string itemDescription;
    public enum ItemType
    {
        healing,
        statBoost,
        info,
    }
    public ItemType itemType;
}
