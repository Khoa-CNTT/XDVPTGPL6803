using System;
using UnityEngine;

[Serializable]
public class EntityData
{
    [SerializeField] string entityID;
    [SerializeField] string entityName;
    [SerializeField] EntityType entityType;
    [SerializeField] EntityLabel entityLabel;
    [SerializeField] string description;

    public string EntityID { get => entityID; set => entityID = value; }
    public string EntityName { get => entityName; set => entityName = value; }
    public EntityType EntityType { get => entityType; set => entityType = value; }
    public EntityLabel EntityLabel { get => entityLabel; set => entityLabel = value; }
    public string Description { get => description; set => description = value; }
}

