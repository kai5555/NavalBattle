using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Ship Stats")]
    public int maxHealth = 500;
    public int currentHealth;

    [Header("UI Elements")]
    public Slider healthSlider;

    private ShipController movementScript;
    private ShipCombat combatScript;
    private AdvancedBuoyancy buoyancyScript;
    private Rigidbody rb;
    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        movementScript = GetComponent<ShipController>();
        combatScript = GetComponent<ShipCombat>();
        buoyancyScript = GetComponent<AdvancedBuoyancy>();
        rb = GetComponent<Rigidbody>();
        if(rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true; // Make the Rigidbody kinematic to prevent physics interactions
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // Already dead, ignore further damage

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

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
        Debug.Log("Player ship has sunk!");
        // Disable player controls
        if (movementScript != null) movementScript.enabled = false;
        if (combatScript != null) combatScript.enabled = false;
        if (buoyancyScript != null) buoyancyScript.enabled = false;

        if (rb != null)
        {
            rb.isKinematic = false; // Allow physics to take over for sinking
            rb.mass = 5000f;
            rb.linearDamping = 2.5f;
            rb.angularDamping = 1.5f;

            float randomRoll = Random.Range(-1f, 1f);
            float randomPitch = Random.Range(-1f, 1f);
            Vector3 tiltForce = new Vector3(randomPitch, 0f, randomRoll) * 15000f;
            rb.AddTorque(tiltForce, ForceMode.Impulse);
        }
        FindAnyObjectByType<GameManager>().TriggerGameOver();

    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (healthSlider != null) healthSlider.value = currentHealth;
    }

    public void UpgradeMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount; // Heal them for the amount they upgraded too
        if (healthSlider != null) 
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }
}
