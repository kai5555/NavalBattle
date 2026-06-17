using UnityEngine;
using TMPro;
using System.Collections;

public class PopupManager : MonoBehaviour
{
    // singleton instance so other scripts can easily call it without needing a reference
    public static PopupManager Instance;

    [Header("UI Reference")]
    public TextMeshProUGUI popupText;
    public float displayTime = 3f;

    void Awake()
    {
        // When game starts, this script registers itself as the master PopupManager
        Instance = this; 
    }

    public void ShowPopup(string message)
    {
        // If a message is already showing, stop timer so we can show new one.
        StopAllCoroutines(); 
        StartCoroutine(PopupRoutine(message));
    }

    private IEnumerator PopupRoutine(string message)
    {
        if (popupText != null)
        {
            popupText.text = message;
            popupText.enabled = true;
        }

        yield return new WaitForSeconds(displayTime);

        if (popupText != null)
        {
            popupText.enabled = false;
        }
    }
}