using UnityEngine;

namespace KLTNLongKhoi
{
    public class UpgradeRecipeDatabase : MonoBehaviour
    {
        public UpgradeRecipeSO[] recipes;

        public UpgradeRecipeSO TryGetInventoryItemSOResult(ItemDataSO weapon, ItemDataSO material)
        {
            foreach (var recipe in recipes)
            {
                if (recipe.targetItem == weapon && recipe.item == material)
                {
                    return recipe;
                }
            }
            return null;
        }
    }
}
