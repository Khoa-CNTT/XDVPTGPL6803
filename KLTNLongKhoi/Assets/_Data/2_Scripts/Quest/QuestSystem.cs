using UnityEngine;
using System.Collections.Generic;

namespace KLTNLongKhoi
{
    public enum QuestType
    {
        Main,
        Side
    }

    public enum QuestStatus
    {
        NotStarted,
        InProgress,
        Completed
    }

    [CreateAssetMenu(fileName = "NewQuest", menuName = "KLTNLongKhoi/Quest")]
    public class Quest : ScriptableObject
    {
        public string questID;
        public string questName;
        public string description;
        public QuestType questType;
        public QuestStatus status;
        public List<QuestObjective> objectives;
        
        [Header("Rewards")]
        public int experienceReward;
        public int moneyReward;
        public List<ItemDataSO> itemRewards;

        public void Initialize()
        {
            status = QuestStatus.NotStarted;
            foreach (var objective in objectives)
            {
                objective.currentAmount = 0;
            }
        }

        public bool CheckCompletion()
        {
            foreach (var objective in objectives)
            {
                if (!objective.IsCompleted())
                {
                    return false;
                }
            }
            status = QuestStatus.Completed;
            return true;
        }
    }

    [System.Serializable]
    public class QuestObjective
    {
        public string objectiveDescription;
        public int requiredAmount;
        public int currentAmount;

        public bool IsCompleted()
        {
            return currentAmount >= requiredAmount;
        }
    }
}