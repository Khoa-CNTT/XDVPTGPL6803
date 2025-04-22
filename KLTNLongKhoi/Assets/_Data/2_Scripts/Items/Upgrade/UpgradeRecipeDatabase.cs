using UnityEngine;

namespace KLTNLongKhoi
{
    public class UpgradeRecipeDatabase : MonoBehaviour
    {
        public UpgradeRecipeSO[] recipes;

        public bool TryGetInventoryItemSOResult(InventoryItemSO weapon, InventoryItemSO material, out InventoryItemSO result,out UpgradeRecipeSO upgradeRecipe)
        {
            foreach (var recipe in recipes)
            {
                if (recipe.targetItem == weapon.itemData && recipe.materials[0].item == material.itemData)
                {
                    result = ScriptableObject.CreateInstance<InventoryItemSO>();
                    result.itemData = recipe.resultItem;
                    result.itemName = recipe.resultItem.itemName;
                    result.icon = recipe.resultItem.icon;
                    result.maxItemsCount = recipe.resultAmount;

                    upgradeRecipe = recipe;
                    return true;
                } 
            } 

            result = null;
            upgradeRecipe = null;
            return false;
        }
    }
}
