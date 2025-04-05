using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{ 
	[SerializeField] List<Item> items = new List<Item>(); // ITEM INVENTORY

	public void AddItem(Item newItem)
	{
		items.Add(newItem);
	}

	public void RemoveItem(Item itemToRemove)
	{
		items.Remove(itemToRemove);
	}

	public List<ItemData> GetItemInventoryData()
	{
		List<ItemData> itemDatas = new List<ItemData>();

		foreach (Item item in items)
		{
			itemDatas.Add(item.ItemData);
		}
		return itemDatas;
	}

	public void SetItemsInventoryByItemsData(List<ItemData> itemsData)
	{
		foreach (var itemData in itemsData)
		{
			//TODO: get item existed/created in world
			Item item = new Item();
			item.ItemData = itemData;
			items.Add(item);
		}
	}
}

