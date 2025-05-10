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
                _icon.color = Color.white; // chỉnh alpha màu icon về 1
                _icon.sprite = cell.ItemDataSO.Icon;
                _itemName.text = cell.ItemDataSO.ItemData.name;
                string description = cell.ItemDataSO.ItemData.description;
                string price = $"Giá: {cell.ItemDataSO.ItemData.price}";
                string stats = "";
                
                if (cell.ItemDataSO.ItemData.physicalDamage > 0)
                    stats += $"\n+Sát thương vật lý: {cell.ItemDataSO.ItemData.physicalDamage}";
                if (cell.ItemDataSO.ItemData.magicDamage > 0)
                    stats += $"\n+Sát thương phép: {cell.ItemDataSO.ItemData.magicDamage}";
                if (cell.ItemDataSO.ItemData.defensePoints > 0)
                    stats += $"\n+Phòng thủ: {cell.ItemDataSO.ItemData.defensePoints}";
                if (cell.ItemDataSO.ItemData.resistance > 0)
                    stats += $"\n+Kháng phép: {cell.ItemDataSO.ItemData.resistance}";
                if (cell.ItemDataSO.ItemData.attackSpeed > 0)
                    stats += $"\n+Tốc độ đánh: {cell.ItemDataSO.ItemData.attackSpeed}";
                if (cell.ItemDataSO.ItemData.healthRecovery > 0)
                    stats += $"\n+Hồi máu: {cell.ItemDataSO.ItemData.healthRecovery}";
                if (cell.ItemDataSO.ItemData.manaRecovery > 0)
                    stats += $"\n+Hồi năng lượng: {cell.ItemDataSO.ItemData.manaRecovery}";
                if (cell.ItemDataSO.ItemData.criticalChance > 0)
                    stats += $"\n+Tỷ lệ chí mạng: {cell.ItemDataSO.ItemData.criticalChance}%";
                
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

            // chỉnh alpha màu icon về 0
            _icon.color = new Color(1, 1, 1, 0);
        }
    }
}
