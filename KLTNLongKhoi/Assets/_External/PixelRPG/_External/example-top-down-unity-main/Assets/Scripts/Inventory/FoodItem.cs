using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "FoodItem", menuName = "Items/FoodItem", order = 2)]
public class FoodItem : Item
{
    public int health;
    public int water;
    public int regeneration;

    public override void OnSelect()
    {
        throw new System.NotImplementedException();
    }
}