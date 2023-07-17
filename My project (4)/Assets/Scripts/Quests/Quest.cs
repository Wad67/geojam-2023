using System;

public class Quest
{
    public string QuestName { get; set; }
    public string Description { get; set; }
    public int ItemGoal { get; set; }
    public int CurrentCount { get; set; }
    public Action OnQuestCompleted = delegate { };

    public Quest(string name, string description, int itemGoal)
    {
        QuestName = name;
        Description = description;
        ItemGoal = itemGoal;
        CurrentCount = 0;
    }

    public bool IsComplete()
    {
        return CurrentCount >= ItemGoal;
    }

    public void IncrementItem()
    {
        CurrentCount++;
        if (IsComplete())
        {
            OnQuestCompleted();
        }
    }
}
