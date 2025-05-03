using System;
using System.Collections.Generic;
using UnityEngine;

namespace KLTNLongKhoi
{
    [CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "KLTNLongKhoi/Crafting Recipe")]
    public class CraftingRecipeSO : ScriptableObject
    {
        [Serializable]
        public class Ingredient
        {
            public ItemDataSO item;
            public int amount;
        }

        public ItemDataSO result;
        public int resultAmount = 1;
        public List<Ingredient> ingredients; // thành phần
    }
}