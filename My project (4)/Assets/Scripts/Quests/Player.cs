using UnityEngine;

public class Player : MonoBehaviour
{
    // reference to the QuestManager
    public QuestManager QuestManager;

    // this method gets called when the player collects an item
    public void CollectItem()
    {
        // other item collection code here...

        // notify the quest system
        QuestManager.CollectItem();
    }
}
