using Parity.SFInventory2.Core;
using UnityEngine;

namespace Parity.SFInventory2.Custom
{
    //class that overrides CanBeDropped method of the cell for custom logic
    public class ItemTypedInventoryCell : InventoryCell
    {
        [SerializeField] private InventoryItem _itemType;

        public override bool CanBeDropped(InventoryCell cell)
        {
            return cell.Item == null || _itemType.GetType() == cell.Item.GetType();
        }

    }
}
