using System;
using UnityEngine;

namespace HieuDev
{
    [Serializable]
    public class ItemData
    {
        private EntityData entityData;

        public EntityData EntityData { get => entityData; set => entityData = value; }
    }
}