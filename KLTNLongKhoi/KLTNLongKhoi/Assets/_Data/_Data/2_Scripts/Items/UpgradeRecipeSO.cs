using System;
using System.Collections.Generic;
using UnityEngine;

namespace KLTNLongKhoi
{
    [CreateAssetMenu(fileName = "New Upgrade Recipe", menuName = "KLTNLongKhoi/Upgrade Recipe")]
    public class UpgradeRecipeSO : ScriptableObject
    {
        [Header("Base Item")]
        public ItemDataSO targetItem;    // Vật phẩm cần nâng cấp
        public int requiredLevel = 1;    // Level yêu cầu của vật phẩm để nâng cấp

        [Header("Materials")]
        public ItemDataSO item; // Vật phẩm làm nguyên liệu
        public int amount;      // Số lượng cần

        [Header("Result")]
        public ItemDataSO resultItem;    // Vật phẩm sau nâng cấp
        public int resultAmount = 1;     // Số lượng nhận được (thường là 1)

        [Header("Settings")]
        [Range(0f, 1f)] public float successRate = 1f; // Tỷ lệ thành công (1 = 100%)
        public bool destroyOnFailure = false; // Có hủy vật phẩm nếu thất bại?
        public bool consumeMaterialsOnFailure = false; // Có tiêu hao nguyên liệu nếu thất bại?
    }
}
