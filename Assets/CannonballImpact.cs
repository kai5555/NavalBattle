using UnityEngine;

public class CannonballImpact : MonoBehaviour
{
    [Header("Impact Visuals")]
    public GameObject splashPrefab;

    [Header("Wave Settings")]
    public float waveAmplitude = 1.5f;
    public float waveFrequency = 2.0f;
    public float waveSpeed = 1.0f;

    private bool hasSplashed = false; 
    private float spawnTime; 

    void Start()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        if (hasSplashed) return;
        if (Time.time < spawnTime + 0.2f) return; // Arming timer

        float currentWaveHeight = Mathf.Sin((transform.position.x + Time.time * waveSpeed) * waveFrequency) * waveAmplitude;

        if (transform.position.y <= currentWaveHeight)
        {
            Explode();
        }
    }

    // NEW: What happens if it hits a solid physical object (like a ship)?
    void OnCollisionEnter(Collision collision)
    {
        if (hasSplashed) return; // Prevent double-explosions

        if (Time.time < spawnTime + 0.1f) return;

        // If the thing we hit is ALSO a cannonball, ignore it and keep flying!
        if (collision.gameObject.GetComponent<CannonballImpact>() != null)
        {
            return;
        }

        // Did we hit an enemy ship?
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(25); // Deal 25 damage per hit!
        }

        Explode();
    }

    // We moved the explosion logic into its own handy function
    void Explode()
    {
        hasSplashed = true;

        if (splashPrefab != null)
        {
            Instantiate(splashPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}