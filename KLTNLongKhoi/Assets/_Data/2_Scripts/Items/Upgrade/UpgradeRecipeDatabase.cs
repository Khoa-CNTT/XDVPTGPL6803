using UnityEngine;

namespace KLTNLongKhoi
{
    public class UpgradeRecipeDatabase : MonoBehaviour
    {
        public UpgradeRecipeSO[] recipes;

        public bool TryGetInventoryItemSOResult(ItemDataSO weapon, ItemDataSO material, out ItemDataSO result,out UpgradeRecipeSO upgradeRecipe)
        {
            foreach (var recipe in recipes)
            {
                if (recipe.targetItem == weapon && recipe.materials[0].item == material)
                {
                    result = ScriptableObject.CreateInstance<ItemDataSO>();
                    result = recipe.resultItem;

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
