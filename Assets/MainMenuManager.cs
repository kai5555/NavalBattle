using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Type the EXACT name of your main game scene here!")]
    public string gameSceneName = "SampleScene";

    public void PlayGame()
    {
        Debug.Log("Loading the ocean...");
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Closing the game...");
        Application.Quit();
    }
}