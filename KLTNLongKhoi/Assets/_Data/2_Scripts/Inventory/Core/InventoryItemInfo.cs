using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace KLTNLongKhoi
{
    //A class that displays information about an item. You can implement IPointerEnterHandler to display information about an item when you hover mouse over cell.
    public class InventoryItemInfo : MonoBehaviour
    {
        private CellsCallbacksController callbacksController;
        private ItemDataSO itemDataSO;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemDescription;
        [SerializeField] private Image _icon;

        public ItemDataSO ItemDataSO { get => itemDataSO; set => itemDataSO = value; }

        private void Awake()
        {
            callbacksController = FindFirstObjectByType<CellsCallbacksController>();
        }

        void Start()
        {
            OnitemDataSONull();
        }

        private void OnEnable()
        {
            callbacksController.onClick += OnClick;
        }

        private void OnDisable()
        {
            callbacksController.onClick -= OnClick;
        }

        private void OnClick(InventoryCell cell, PointerEventData eventData)
        {
            if (_itemName == null || _itemDescription == null || _icon == null) return;
            if (cell != null && cell.ItemDataSO != null)
            {
                ItemDataSO = cell.ItemDataSO;
                _icon.sprite = cell.ItemDataSO.icon;
                _itemName.text = cell.ItemDataSO.itemData.name;
                string description = cell.ItemDataSO.itemData.description;
                string price = $"Giá: {cell.ItemDataSO.itemData.price}";
                string stats = "";
                
                if (cell.ItemDataSO.itemData.physicalDamage > 0)
                    stats += $"\n+Sát thương vật lý: {cell.ItemDataSO.itemData.physicalDamage}";
                if (cell.ItemDataSO.itemData.magicDamage > 0)
                    stats += $"\n+Sát thương phép: {cell.ItemDataSO.itemData.magicDamage}";
                if (cell.ItemDataSO.itemData.defensePoints > 0)
                    stats += $"\n+Phòng thủ: {cell.ItemDataSO.itemData.defensePoints}";
                if (cell.ItemDataSO.itemData.resistance > 0)
                    stats += $"\n+Kháng phép: {cell.ItemDataSO.itemData.resistance}";
                if (cell.ItemDataSO.itemData.attackSpeed > 0)
                    stats += $"\n+Tốc độ đánh: {cell.ItemDataSO.itemData.attackSpeed}";
                if (cell.ItemDataSO.itemData.healthRecovery > 0)
                    stats += $"\n+Hồi máu: {cell.ItemDataSO.itemData.healthRecovery}";
                if (cell.ItemDataSO.itemData.manaRecovery > 0)
                    stats += $"\n+Hồi năng lượng: {cell.ItemDataSO.itemData.manaRecovery}";
                if (cell.ItemDataSO.itemData.criticalChance > 0)
                    stats += $"\n+Tỷ lệ chí mạng: {cell.ItemDataSO.itemData.criticalChance}%";
                
                _itemDescription.text = $"{description}\n{price}{stats}";
            }
        }

        // trường hợp không có gì
        private void OnitemDataSONull()
        {
            ItemDataSO = null;
            _icon.sprite = null;
            _itemName.text = "";
            _itemDescription.text = "";
        }
    }
}
