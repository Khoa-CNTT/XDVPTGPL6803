using UnityEngine;

namespace KLTNLongKhoi
{
    [CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "KLTNLongKhoi/Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public InventoryItem[] ingredients = new InventoryItem[3];
        public InventoryItem result;
        public string recipeName;
        [TextArea(3,5)]
        public string description;

        public bool MatchesIngredients(InventoryItem item1, InventoryItem item2, InventoryItem item3)
        {
            InventoryItem[] inputItems = { item1, item2, item3 };
            
            // Kiểm tra từng nguyên liệu
            for (int i = 0; i < ingredients.Length; i++)
            {
                if (ingredients[i] == null && inputItems[i] != null) return false;
                if (ingredients[i] != null && inputItems[i] == null) return false;
                if (ingredients[i] != null && !ingredients[i].Equals(inputItems[i])) return false;
            }
            
            return true;
        }
    }
}