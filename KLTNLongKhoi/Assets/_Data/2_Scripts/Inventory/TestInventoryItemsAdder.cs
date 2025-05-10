using Parity.SFInventory2.Core;
using TMPro;
using UnityEngine;
using Parity.SFInventory2.Testing;

namespace KLTNLongKhoi
{
    //An example that shows how to add items to the inventory and how to handle the logic of adding items
    public class TestInventoryItemsAdder : MonoBehaviour
    {
        [SerializeField] private AddingItem[] addingItems;
        [SerializeField] private ContainerBase _container;
        [SerializeField] private TextMeshProUGUI _text;

        private void Start()
        {
            foreach (var item in addingItems)
            {
                // _text.text += item.key.ToString() + " - " + item.item.itemName + '\n';
            }
        }

        void Update()
        {
            for (int i = 0; i < addingItems.Length; i++)
            {
                if (Input.GetKeyDown(addingItems[i].key))
                {
                    _container.AddItemsCount(addingItems[i].item, addingItems[i].item.ItemData.maxStack, out var itemsLeft);
                    if (itemsLeft > 0)
                    {
                        //this is where you can throw away remaining items
                        Debug.Log($"Not enough space! {itemsLeft} items left!");
                    }
                }
            }
        }
        [System.Serializable]
        public class AddingItem
        {
            public ItemDataSO item;
            public KeyCode key;
        }
    }
}