using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 1)]
public abstract class Item : ScriptableObject
{
    public new string name;
    public int id;
    public bool isStackable;
    public Sprite sprite;

    public bool isSelected;

    public abstract void OnSelect();

    public void Select()
    {
        isSelected = !isSelected;
        OnSelect();
    }
}