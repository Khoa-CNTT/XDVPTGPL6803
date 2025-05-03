using UnityEngine;

namespace KLTNLongKhoi
{
    [CreateAssetMenu(fileName = "New Item", menuName = "KLTNLongKhoi/Item Data")]
    public class ItemDataSO : ScriptableObject
    {
        [Header("Basic Info")]
        public ItemData itemData;
        public Sprite icon;
        public GameObject prefab;
    }
}