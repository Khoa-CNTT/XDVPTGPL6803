using System.Collections.Generic;
using UnityEngine;

public class EntityPool : MonoBehaviour
{
    public List<Entity> listEntity = new List<Entity>();

    public Entity GetReusableEntity(EntityLabel entityLabel)
    {
        foreach (Entity entity in listEntity)
        {
            if (entity.IsDestroyed && entity.EntityData.EntityLabel == entityLabel)
            {
                entity.ResetEntity();
                return entity;
            }
        }
        return null;
    }

    public void AddEnitity(Entity entity)
    {
        listEntity.Add(entity);
    }

    public void DestroyEntity(Entity entity)
    {
        foreach (Entity e in listEntity)
        {
            if (e == entity) e.DestroyEntity();
        }
    }
}

