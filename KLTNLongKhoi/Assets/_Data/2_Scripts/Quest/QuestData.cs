using UnityEngine;

namespace KLTNLongKhoi
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
    public class QuestData : ScriptableObject
    {
        public string questID;
        public string questName;
        [TextArea(3, 10)]
        public string description;
        public int requiredAmount;
        public QuestData[] prerequisiteQuests;
        public int goldReward;
        public ItemData[] itemRewards;
    }
}