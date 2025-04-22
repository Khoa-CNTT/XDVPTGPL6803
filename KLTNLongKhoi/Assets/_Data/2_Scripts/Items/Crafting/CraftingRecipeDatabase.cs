using UnityEngine;

namespace KLTNLongKhoi
{ 
    public class CraftingRecipeDatabase : MonoBehaviour
    {
        public CraftingRecipeSO[] recipes;

        public InventoryItemSO FindRecipe(InventoryItemSO item1, InventoryItemSO item2, InventoryItemSO item3)
        {
            foreach (var recipe in recipes)
            {
                // Kiểm tra các trường hợp sắp xếp khác nhau của nguyên liệu
                if ((recipe.ingredients[0].item == item1.itemData && 
                     recipe.ingredients[1].item == item2.itemData && 
                     recipe.ingredients[2].item == item3.itemData) ||
                    
                    (recipe.ingredients[0].item == item1.itemData && 
                     recipe.ingredients[1].item == item3.itemData && 
                     recipe.ingredients[2].item == item2.itemData) ||
                    
                    (recipe.ingredients[0].item == item2.itemData && 
                     recipe.ingredients[1].item == item1.itemData && 
                     recipe.ingredients[2].item == item3.itemData) ||
                    
                    (recipe.ingredients[0].item == item2.itemData && 
                     recipe.ingredients[1].item == item3.itemData && 
                     recipe.ingredients[2].item == item1.itemData) ||
                    
                    (recipe.ingredients[0].item == item3.itemData && 
                     recipe.ingredients[1].item == item1.itemData && 
                     recipe.ingredients[2].item == item2.itemData) ||
                    
                    (recipe.ingredients[0].item == item3.itemData && 
                     recipe.ingredients[1].item == item2.itemData && 
                     recipe.ingredients[2].item == item1.itemData))
                {
                    // Tạo một instance mới của InventoryItemSO
                    InventoryItemSO result = ScriptableObject.CreateInstance<InventoryItemSO>();
                    result.itemData = recipe.result;
                    result.itemName = recipe.result.itemName;
                    result.icon = recipe.result.icon;
                    result.maxItemsCount = recipe.resultAmount;
                    return result;
                }
            }
            return null;
        } 
    }
}
