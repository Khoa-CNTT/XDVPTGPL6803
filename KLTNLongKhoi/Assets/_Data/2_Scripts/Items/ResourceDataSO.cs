using UnityEngine;

namespace KLTNLongKhoi
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Inventory/Resource")]
    public class ResourceDataSO : ItemDataSO
    {
        public ResourceType resourceType;

        private void OnValidate()
        {
            itemType = ItemType.Resource;
        }
    }
}