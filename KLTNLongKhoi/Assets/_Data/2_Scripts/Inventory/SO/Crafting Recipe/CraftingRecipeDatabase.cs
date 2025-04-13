using UnityEngine;

namespace KLTNLongKhoi
{
    [CreateAssetMenu(fileName = "Crafting Recipe Database", menuName = "KLTNLongKhoi/Crafting Recipe Database")]
    public class CraftingRecipeDatabase : ScriptableObject
    {
        public CraftingRecipe[] recipes;

        public CraftingRecipe FindRecipe(InventoryItem item1, InventoryItem item2, InventoryItem item3)
        {
            foreach (var recipe in recipes)
            {
                if (recipe.MatchesIngredients(item1, item2, item3))
                {
                    return recipe;
                }
            }
            return null;
        }
    }
}