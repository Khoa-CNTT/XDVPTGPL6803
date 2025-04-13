using UnityEngine;

namespace KLTNLongKhoi
{
    [CreateAssetMenu(fileName = "Upgrade Recipe Database", menuName = "KLTNLongKhoi/Upgrade Recipe Database")]
    public class UpgradeRecipeDatabase : ScriptableObject
    {
        public UpgradeRecipe[] recipes;

        public UpgradeRecipe FindRecipe(InventoryItem weapon, InventoryItem material)
        {
            foreach (var recipe in recipes)
            {
                if (recipe.baseWeapon == weapon && recipe.upgradeMaterial == material)
                {
                    return recipe;
                }
            }
            return null;
        }
    }
}