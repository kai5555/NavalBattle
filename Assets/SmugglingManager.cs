using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SmugglingManager : MonoBehaviour
{
    [Header("Economy")]
    public int gold = 0;
    public string currentDestination = "None";
    public int bonusDamage = 0;

    [Header("Upgrade Costs")]
    public int repairCost = 50;
    public int healthUpgradeCost = 150;
    public int damageUpgradeCost = 200;

    [Header("References")]
    public PlayerHealth healthScript;
    public Transform navigationArrow;

    [Header("UI Elements")]
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI destinationText;
    public TextMeshProUGUI shopPromptText;

    [Header("Audio")]
    public AudioClip coinSound;
    private AudioSource uiAudioSpeaker;

    private string[] allIslands = { "Island_Nassau", "Island_Tortuga", "Island_Havana" };
    private bool isInPort = false;
    private Transform currentTargetTransform;

    void Start()
    {
        uiAudioSpeaker = gameObject.AddComponent<AudioSource>();
        UpdateUI();
        if (shopPromptText != null) shopPromptText.enabled = false;
        if (navigationArrow != null) 
        {
            navigationArrow.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (navigationArrow != null && currentTargetTransform != null)
        {
            // Find where island is
            Vector3 targetPos = currentTargetTransform.position;
            // Lock the Y axis so arrow stays perfectly flat and doesnt tilt into water.
            targetPos.y = navigationArrow.position.y; 
            // Force arrow to point at target island
            navigationArrow.LookAt(targetPos);
        }
        if (Keyboard.current == null) return;

        if (isInPort)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame && gold >= repairCost)
            {
                healthScript.Heal(100);
                gold -= repairCost;
                UpdateUI();
            }
            if (Keyboard.current.digit2Key.wasPressedThisFrame && gold >= healthUpgradeCost)
            {
                healthScript.UpgradeMaxHealth(100);
                gold -= healthUpgradeCost;
                UpdateUI();
            }
            if (Keyboard.current.digit3Key.wasPressedThisFrame && gold >= damageUpgradeCost)
            {
                bonusDamage += 10;
                gold -= damageUpgradeCost;
                UpdateUI();
            }
        }
    }

    public void UpdateUI()
    {
        if (goldText != null) goldText.text = "Gold: " + gold;
        if (destinationText != null) destinationText.text = "Destination: " + currentDestination.Replace("Island_", "");
    }

    // When you sail into a Harbor Zone
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Port"))
        {
            isInPort = true;
            string portName = other.transform.parent.name; // Gets the name of the Island
            if (shopPromptText != null) shopPromptText.enabled = true; // Show shop prompt

            // Did we deliver the cargo?
            if (portName == currentDestination)
            {
                int reward = Random.Range(100, 250);
                gold += reward;
                currentDestination = "None";
                UpdateUI();
                if (coinSound != null) uiAudioSpeaker.PlayOneShot(coinSound);
                if (PopupManager.Instance != null)
                {
                    PopupManager.Instance.ShowPopup("Delivery Success! +" + reward + " Gold");
                }
            }

            // Assign new mission if we don't have one
            if (currentDestination == "None")
            {
                AssignNewMission(portName);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Port"))
        {
            isInPort = false;
            if (shopPromptText != null) shopPromptText.enabled = false;
        }
    }

    void AssignNewMission(string currentPort)
    {
        string newTarget = currentPort;
        // Keep picking random island until not the one player is currently at
        while (newTarget == currentPort)
        {
            newTarget = allIslands[Random.Range(0, allIslands.Length)];
        }
        
        currentDestination = newTarget;
        UpdateUI();
        GameObject targetObj = GameObject.Find(currentDestination);
        if (targetObj != null)
        {
            currentTargetTransform = targetObj.transform;
            if (navigationArrow != null) navigationArrow.gameObject.SetActive(true); 
        }
    }
}