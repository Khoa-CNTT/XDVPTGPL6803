using UnityEngine;

namespace KLTNLongKhoi
{
    [CreateAssetMenu(fileName = "New Item", menuName = "KLTNLongKhoi/Item Data")]
    public class ItemDataSO : ScriptableObject
    {
        [Header("Basic Info")]
        [SerializeField] private ItemData itemData;
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObject prefab;

        public ItemData ItemData { get => itemData; private set => itemData = value; }
        public Sprite Icon { get => icon; private set => icon = value; }
        public GameObject Prefab { get => prefab; private set => prefab = value; }
    }
}