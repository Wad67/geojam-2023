using UnityEngine;

public class QuestTest : MonoBehaviour
{
    public QuestManager questManager;

    void Start()
    {
        Quest testQuest = new Quest("Test Quest", "Collect 5 items", 5);
        questManager.AssignQuest(testQuest);
    }
}


