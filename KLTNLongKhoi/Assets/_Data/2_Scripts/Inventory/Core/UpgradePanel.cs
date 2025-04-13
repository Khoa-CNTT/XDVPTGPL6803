using UnityEngine;
using UnityEngine.Events;

namespace KLTNLongKhoi
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private InventoryCell weaponCell;        // Slot cho vũ khí
        [SerializeField] private InventoryCell materialCell;      // Slot cho nguyên liệu
        [SerializeField] private InventoryController targetContainer;   // Container để nhận item nâng cấp
        
        [Header("Upgrade System")]
        [SerializeField] private UpgradeRecipeDatabase recipeDatabase;
        
        [Header("Events")]
        public UnityEvent onUpgradeSuccess;
        public UnityEvent onUpgradeFail;

        private void Awake()
        {
            targetContainer = FindFirstObjectByType<InventoryController>();
        }

        public void TryUpgrade()
        {
            if (weaponCell.Item == null || materialCell.Item == null) return;

            var recipe = recipeDatabase.FindRecipe(weaponCell.Item, materialCell.Item);
            
            if (recipe != null)
            {
                bool success = Random.value <= recipe.upgradeChance;
                
                if (success)
                {
                    // Thêm vũ khí mới vào container
                    targetContainer.AddItemsCount(recipe.resultWeapon, recipe.resultWeapon.maxItemsCount, out var itemsLeft);
                    
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
