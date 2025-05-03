[System.Serializable]
public class QuestStatus
{
    public string questID;
    public bool isActive;
    public bool isCompleted;
    public int currentAmount;

    public QuestStatus(string id)
    {
        questID = id;
        isActive = false;
        isCompleted = false;
        currentAmount = 0;
    }
}