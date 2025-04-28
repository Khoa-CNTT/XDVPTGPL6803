using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField] private Button upgradeButton;

        [Header("Upgrade System")]
        [SerializeField] private UpgradeRecipeDatabase recipeDatabase;

        [Header("Events")]
        public UnityEvent onUpgradeSuccess;
        public UnityEvent onUpgradeFail;

        private void Awake()
        {
            targetContainer = FindFirstObjectByType<InventoryController>();
            recipeDatabase = FindFirstObjectByType<UpgradeRecipeDatabase>();
            upgradeButton.onClick.AddListener(TryUpgrade);
        }

        public void TryUpgrade()
        {
            if (weaponCell.ItemDataSO == null || materialCell.ItemDataSO == null) return;

            UpgradeRecipeSO result = recipeDatabase.TryGetInventoryItemSOResult(weaponCell.ItemDataSO, materialCell.ItemDataSO);

            if (result != null)
            {
                bool success = Random.value <= result.successRate; // Tỷ lệ thành công

                if (success)
                {
                    // Thêm vũ khí mới vào container
                    targetContainer.AddItemsCount(result.resultItem, result.resultAmount, out var itemsLeft);

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
