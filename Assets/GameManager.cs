using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    public void TriggerGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            UnlockCursor();
            LockPlayerControls();
        }
    }

    public void TriggerVictory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            UnlockCursor();
            LockPlayerControls();
        }
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
