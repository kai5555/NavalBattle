using UnityEngine;
using UnityEngine.UI; // We need this line to talk to the UI!

public class EnemyHealth : MonoBehaviour
{
    [Header("Ship Stats")]
    public int maxHealth = 500;
    private int currentHealth;

    [Header("UI")]
    public Slider healthSlider;     // The visual bar
    public Transform healthCanvas;  // The canvas we need to rotate

    private AdvancedBuoyancy buoyancyScript;
    private Rigidbody rb;
    private Transform playerCamera;

    void Start()
    {
        currentHealth = maxHealth;
        
        // Setup the UI slider at the start
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
        // BILLBOARD EFFECT: Make the canvas always face the camera
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

        // Update the visual slider
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
        // Hide the health bar when the ship dies
        if (healthCanvas != null) healthCanvas.gameObject.SetActive(false);

        if (buoyancyScript != null) buoyancyScript.enabled = false;

        if (rb != null)
        {
            rb.isKinematic = false; 
            rb.mass = 5000f; 
            rb.linearDamping = 2.5f;  
            rb.angularDamping = 1.5f;

            float randomRoll = Random.Range(-1f, 1f);
            float randomPitch = Random.Range(-1f, 1f);

            Vector3 tiltForce = new Vector3(randomPitch, 0f, randomRoll) * 15000f;
            rb.AddTorque(tiltForce, ForceMode.Impulse);
        }

        Destroy(gameObject, 15f);
        FindAnyObjectByType<GameManager>().TriggerVictory();
    }
}