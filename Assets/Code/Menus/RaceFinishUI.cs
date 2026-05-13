using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceFinishUI : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu";

    public void PlayAgain()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}