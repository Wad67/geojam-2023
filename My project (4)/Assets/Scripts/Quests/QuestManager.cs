using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public Quest CurrentQuest;

    public TMP_Text QuestNameText;
    public TMP_Text QuestDescriptionText;
    public Slider QuestProgressSlider;

    public void AssignQuest(Quest quest)
    {
        CurrentQuest = quest;
        CurrentQuest.OnQuestCompleted += CompleteQuest;

        // Update UI
        QuestNameText.text = CurrentQuest.QuestName;
        QuestDescriptionText.text = CurrentQuest.Description;
        QuestProgressSlider.maxValue = CurrentQuest.ItemGoal;
        QuestProgressSlider.value = CurrentQuest.CurrentCount;
    }

    public void CompleteQuest()
    {
        // put here any rewards or completion logic
        Debug.Log($"Quest '{CurrentQuest.QuestName}' has been completed!");
        CurrentQuest = null;

        // Clear UI
        QuestNameText.text = "";
        QuestDescriptionText.text = "";
        QuestProgressSlider.value = 0;
    }

    public void CollectItem()
    {
        if (CurrentQuest != null)
        {
            CurrentQuest.IncrementItem();
            Debug.Log($"Item collected! Current count: {CurrentQuest.CurrentCount}");

            // Update UI
            QuestProgressSlider.value = CurrentQuest.CurrentCount;
        }
    }
}

