using Parity.SFInventory2.Core;
using UnityEngine;

namespace Parity.SFInventory2.Custom
{
    //just an override of base item
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/InventoryItems/ArmorItem", order = 1)]
    public class ArmorItem : InventoryItem
    {
        public float defensePoints = 5;
    }
}