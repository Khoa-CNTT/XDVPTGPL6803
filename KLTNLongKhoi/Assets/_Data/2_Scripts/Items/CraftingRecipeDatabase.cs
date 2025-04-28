using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace KLTNLongKhoi
{
    public class CraftingRecipeDatabase : MonoBehaviour
    {
        public CraftingRecipeSO[] recipes;

        public ItemDataSO FindRecipe(ItemDataSO item1, ItemDataSO item2, ItemDataSO item3)
        {
            foreach (var recipe in recipes)
            {
                if (recipe.ingredients == null || recipe.ingredients.Count == 0 ||
                    (item1 == null && item2 == null && item3 == null))
                    continue;

                // Lọc ra các item input không null
                var inputItems = new List<ItemDataSO>();
                if (item1 != null) inputItems.Add(item1);
                if (item2 != null) inputItems.Add(item2);
                if (item3 != null) inputItems.Add(item3);

                // Lấy danh sách nguyên liệu từ công thức
                var recipeItems = recipe.ingredients
                    .Where(ing => ing?.item != null)
                    .Select(ing => ing.item)
                    .ToList();

                // Kiểm tra số lượng nguyên liệu có khớp không
                if (inputItems.Count != recipeItems.Count)
                    continue;

                // Kiểm tra xem tất cả các item có match với công thức không (bất kể thứ tự)
                if (HasSameElements(recipeItems.ToArray(), inputItems.ToArray()))
                {
                    // Tạo một instance mới của ItemDataSO
                    ItemDataSO result = ScriptableObject.CreateInstance<ItemDataSO>();
                    result = recipe.result;
                    return result;
                }
            }
            return null;
        }

        private bool HasSameElements(ItemDataSO[] a, ItemDataSO[] b)
        {
            if (a.Length != b.Length) return false;
            
            var used = new bool[a.Length];
            
            foreach (var item1 in a)
            {
                bool found = false;
                for (int i = 0; i < b.Length; i++)
                {
                    if (!used[i] && b[i] == item1)
                    {
                        used[i] = true;
                        found = true;
                        break;
                    }
                }
                if (!found) return false;
            }
            return true;
        }
    }
}
