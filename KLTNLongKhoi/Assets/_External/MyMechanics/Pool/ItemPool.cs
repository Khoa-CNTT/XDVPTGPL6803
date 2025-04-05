using UnityEngine;

public class ItemPool : EntityPool
{
    public ItemsPrefab itemsPrefab;

    public Item CreateItemByEntityLabel(EntityLabel entityLabel)
    {
        Entity newEntity = GetReusableEntity(entityLabel);
        Item newItem = null;
        if (newEntity == null) // không tái sử dụng được
        {
            newItem = itemsPrefab.GetItemByEntityLabel(entityLabel);
            if (newItem != null)
            {
                newItem = Instantiate(newItem.gameObject, transform.position, Quaternion.identity, transform).GetComponent<Item>();
                AddEnitity(newItem);
            }
        }
        else // tái sử dụng được
        {
            newItem = newEntity.GetComponent<Item>();
        }
        return newItem;
    }
}

