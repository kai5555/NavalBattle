using UnityEngine;
using UnityEngine.SceneManagement; // We need this to load levels!

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Type the EXACT name of your main game scene here!")]
    public string gameSceneName = "SampleScene"; // Change this if your scene is named SampleScene!

    // This function will be triggered by the Start Button
    public void PlayGame()
    {
        Debug.Log("Loading the ocean...");
        SceneManager.LoadScene(gameSceneName);
    }

    // This function will be triggered by the Quit Button
    public void QuitGame()
    {
        Debug.Log("Closing the game...");
        Application.Quit(); // This closes the actual game (.exe)
    }
}