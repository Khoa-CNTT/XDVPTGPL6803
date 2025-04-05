using System;
using UnityEngine;

[Serializable]
public class ItemData
{
    private EntityData entityData;

    public EntityData EntityData { get => entityData; set => entityData = value; }
}
