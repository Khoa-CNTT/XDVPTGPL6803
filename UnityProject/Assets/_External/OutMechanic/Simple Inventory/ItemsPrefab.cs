using System.Collections.Generic;
using UnityEngine;

/// <summary> Chứa các prefab với item data được tạo sẵn </summary>
public class ItemsPrefab : MonoBehaviour
{
	public List<Item> items = new List<Item>();

    public Item GetItemByEntityLabel(EntityLabel entityLabel)
	{  
		foreach (Item item in items)
		{
			if (item.EntityData.EntityLabel == entityLabel)
			{
				return item;
			}
		} 
		return null;
	}


}

