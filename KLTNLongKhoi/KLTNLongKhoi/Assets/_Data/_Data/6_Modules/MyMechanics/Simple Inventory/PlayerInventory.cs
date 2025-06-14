using UnityEngine;

namespace HieuDev
{
    public class PlayerInventory : Inventory
    {
        public ItemPool itemPool;

        void Start()
        {
            itemPool = FindFirstObjectByType<ItemPool>();
        }

        void Update()
        {
            XuLyThemItem();
        }

        private void XuLyThemItem()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Item newItem = itemPool.CreateItemByEntityLabel(EntityLabel.Object_1);
                if (newItem != null)
                {
                    AddItem(newItem);
                }
            }
        }
    }

}
