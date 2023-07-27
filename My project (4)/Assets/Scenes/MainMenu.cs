using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");  // "GameScene" is the name of your main game scene.
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("CreditsScene"); // "CreditsScene" is the name of your credits scene.
    }
}
