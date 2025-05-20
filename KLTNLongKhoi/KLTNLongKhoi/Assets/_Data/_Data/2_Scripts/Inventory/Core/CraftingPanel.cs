using UnityEngine;

namespace KLTNLongKhoi
{
    public class CraftingPanel : MonoBehaviour
    {
        [SerializeField] InventoryCell inventoryCell1;
        [SerializeField] InventoryCell inventoryCell2;
        [SerializeField] InventoryCell inventoryCell3;
        [SerializeField] InventoryCell inventoryCellResult;
        
        [Header("Crafting System")]
        [SerializeField] CraftingRecipeDatabase recipeDatabase;

        bool isCrafted;

        void Start()
        {
            recipeDatabase = FindFirstObjectByType<CraftingRecipeDatabase>();
        }

        void Update()
        {
            Craft();
            CheckCraftResult();
        }

        private void CheckCraftResult()
        {
            if (isCrafted && inventoryCellResult.ItemDataSO == null)
            {
                ClearIngredients();
                isCrafted = false;
            }
        }

        private void ClearIngredients()
        {
            inventoryCell1.SetInventoryItem(null);
            inventoryCell2.SetInventoryItem(null);
            inventoryCell3.SetInventoryItem(null);
            
            inventoryCell1.UpdateCellUI();
            inventoryCell2.UpdateCellUI();
            inventoryCell3.UpdateCellUI();
        }

        public void Craft()
        {
            if (inventoryCellResult.ItemDataSO != null || isCrafted) return;

            var recipe = recipeDatabase.FindRecipe(
                inventoryCell1.ItemDataSO,
                inventoryCell2.ItemDataSO,
                inventoryCell3.ItemDataSO
            );

            if (recipe != null)
            {
                inventoryCellResult.SetInventoryItem(recipe);
                inventoryCellResult.UpdateCellUI();
                isCrafted = true;
                GameEvents.Notify(GameEventType.ItemCrafted);
            }
        }
    }
}
