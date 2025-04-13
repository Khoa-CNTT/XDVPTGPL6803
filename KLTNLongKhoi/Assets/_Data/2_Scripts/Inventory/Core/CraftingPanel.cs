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

        void Update()
        {
            Craft();
            CheckCraftResult();
        }

        private void CheckCraftResult()
        {
            if (isCrafted && inventoryCellResult.Item == null)
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
            if (inventoryCellResult.Item != null || isCrafted) return;

            var recipe = recipeDatabase.FindRecipe(
                inventoryCell1.Item,
                inventoryCell2.Item,
                inventoryCell3.Item
            );

            if (recipe != null)
            {
                inventoryCellResult.SetInventoryItem(recipe.result);
                inventoryCellResult.UpdateCellUI();
                isCrafted = true;
            }
        }
    }
}
