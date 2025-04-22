using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace KLTNLongKhoi
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private InventoryCell weaponCell;        // Slot cho vũ khí
        [SerializeField] private InventoryCell materialCell;      // Slot cho nguyên liệu
        [SerializeField] private InventoryController targetContainer;   // Container để nhận item nâng cấp
        [SerializeField] private TextMeshProUGUI upgradeChanceText;
        
        [Header("Upgrade System")]
        [SerializeField] private UpgradeRecipeDatabase recipeDatabase;
        
        [Header("Events")]
        public UnityEvent onUpgradeSuccess;
        public UnityEvent onUpgradeFail;

        private void Awake()
        {
            targetContainer = FindFirstObjectByType<InventoryController>();
            recipeDatabase = FindFirstObjectByType<UpgradeRecipeDatabase>();
        }
        
        private void Update()
        {
            UpdateUpgradeChance();
        }

        private void UpdateUpgradeChance()
        {
            if (weaponCell.Item != null && materialCell.Item != null)
            {
                var result = recipeDatabase.TryGetInventoryItemSOResult(weaponCell.Item, materialCell.Item, out var itemInventoryResult, out var upgradeRecipe);

                if (result)
                {
                    upgradeChanceText.text = $"Upgrade Chance: {upgradeRecipe.successRate * 100}%";
                }
                else
                {
                    upgradeChanceText.text = "Upgrade Chance: 0%";
                }
            }
        }

        public void TryUpgrade()
        {
            if (weaponCell.Item == null || materialCell.Item == null) return;

            var result = recipeDatabase.TryGetInventoryItemSOResult(weaponCell.Item, materialCell.Item, out var itemInventoryResult, out var upgradeRecipe);

            if (result)
            {
                bool success = Random.value <= upgradeRecipe.successRate; // Tỷ lệ thành công

                if (success)
                {
                    // Thêm vũ khí mới vào container
                    targetContainer.AddItemsCount(itemInventoryResult, itemInventoryResult.maxItemsCount, out var itemsLeft);

                    if (itemsLeft <= 0)
                    {
                        // Xóa các nguyên liệu khi thêm thành công
                        ClearCells();
                        onUpgradeSuccess?.Invoke();
                    }
                    else
                    {
                        Debug.Log($"Not enough space! {itemsLeft} items left!");
                    }
                }
                else
                {
                    // Thất bại - chỉ mất nguyên liệu
                    materialCell.SetInventoryItem(null);
                    materialCell.UpdateCellUI();
                    onUpgradeFail?.Invoke();
                }
            }
        }

        private void ClearCells()
        {
            weaponCell.SetInventoryItem(null);
            materialCell.SetInventoryItem(null);
            
            weaponCell.UpdateCellUI();
            materialCell.UpdateCellUI();
        }
    }
}
