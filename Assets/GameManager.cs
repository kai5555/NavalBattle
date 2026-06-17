using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    private bool isPaused = false;

    void Start()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // Don't allow pausing if player is dead
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
            Time.timeScale = 0f; // freeze game
            UnlockCursor();
        }
        else
        {
            Time.timeScale = 1f; // unfreeze
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
