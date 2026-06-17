using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Ship Stats")]
    public int maxHealth = 500;
    private int currentHealth;

    [Header("UI")]
    public Slider healthSlider;  
    public Transform healthCanvas;

    private AdvancedBuoyancy buoyancyScript;
    private Rigidbody rb;
    private Transform playerCamera;

    void Start()
    {
        currentHealth = maxHealth;
        
        // Setup UI slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        // Find the main camera so the health bar can look at it
        playerCamera = Camera.main.transform;

        buoyancyScript = GetComponent<AdvancedBuoyancy>();
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true; 
    }

    void Update()
    {
        // Make canvas always face the camera
        if (healthCanvas != null && playerCamera != null)
        {
            healthCanvas.LookAt(healthCanvas.position + playerCamera.rotation * Vector3.forward,
                                playerCamera.rotation * Vector3.up);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return; // Stop taking damage if already dead

        currentHealth -= damageAmount;

        // Update visual slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Sink();
        }
    }

    void Sink()
    {
        Debug.Log("Enemy Ship Destroyed!");
        
        // Bounty system
        SmugglingManager playerEconomy = FindAnyObjectByType<SmugglingManager>();
        if (playerEconomy != null)
        {
            int bounty = Random.Range(100, 200);
            playerEconomy.gold += bounty;
            Debug.Log("Claimed Pirate Bounty: " + bounty + " Gold! Total: " + playerEconomy.gold);
        }
        // Hide health bar when dead
        if (healthCanvas != null) healthCanvas.gameObject.SetActive(false);

        if (buoyancyScript != null) buoyancyScript.enabled = false;

        if (rb != null)
        {
            rb.isKinematic = false; // Allow physics to take over
            rb.mass = 5000f; // Make it heavy so it sinks
            rb.linearDamping = 2.5f;  // dampen movement to simulate water resistance
            rb.angularDamping = 1.5f; // dampen rotation to simulate water resistance

            float randomRoll = Random.Range(-1f, 1f);
            float randomPitch = Random.Range(-1f, 1f);

            Vector3 tiltForce = new Vector3(randomPitch, 0f, randomRoll) * 15000f;
            rb.AddTorque(tiltForce, ForceMode.Impulse);
        }

        Destroy(gameObject, 15f);
    }
}