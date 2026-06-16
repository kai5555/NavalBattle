using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Needed to read the Escape key!

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject pausePanel; // NEW: The Pause Menu

    private bool isPaused = false;

    void Start()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    void Update()
    {
        // Safety check
        if (Keyboard.current == null) return;

        // If we press ESCAPE...
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // Don't allow pausing if the player is already dead!
            if (gameOverPanel != null && gameOverPanel.activeSelf) return;

            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        
        if (pausePanel != null) pausePanel.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f; // FREEZES THE GAME!
            UnlockCursor();
        }
        else
        {
            Time.timeScale = 1f; // UNFREEZES THE GAME!
            LockCursor(); 
        }
    }

    public void ResumeGame()
    {
        TogglePause();
    }

    public void TriggerGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            UnlockCursor();
            LockPlayerControls();
        }
    }

    public void QuitToTitle()
    {
        // CRUCIAL: Always unfreeze time before loading a new level, 
        // or your Main Menu will be permanently frozen!
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void LockPlayerControls()
    {
        ShipController shipController = FindAnyObjectByType<ShipController>();
        if (shipController != null)
        {
            shipController.enabled = false;
        }

        ShipCombat shipCombat = FindAnyObjectByType<ShipCombat>();
        if (shipCombat != null)
        {
            shipCombat.enabled = false;
        }
    }

    public void RestartBattle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
