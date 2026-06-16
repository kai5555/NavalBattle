using UnityEngine;
using TMPro;
using System.Collections;

public class PopupManager : MonoBehaviour
{
    // The "Singleton" - this allows ANY script to find this manager instantly!
    public static PopupManager Instance;

    [Header("UI Reference")]
    public TextMeshProUGUI popupText;
    public float displayTime = 3f; // How long the message stays on screen

    void Awake()
    {
        // When the game starts, this script registers itself as the master PopupManager
        Instance = this; 
    }

    public void ShowPopup(string message)
    {
        // If a message is already showing, stop that timer so we can show the new one!
        StopAllCoroutines(); 
        StartCoroutine(PopupRoutine(message));
    }

    private IEnumerator PopupRoutine(string message)
    {
        // 1. Set the text and turn it on
        if (popupText != null)
        {
            popupText.text = message;
            popupText.enabled = true;
        }

        // 2. Wait for 3 seconds
        yield return new WaitForSeconds(displayTime);

        // 3. Turn it back off
        if (popupText != null)
        {
            popupText.enabled = false;
        }
    }
}