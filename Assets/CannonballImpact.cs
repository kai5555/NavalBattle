using UnityEngine;

public class CannonballImpact : MonoBehaviour
{
    [Header("Impact Visuals")]
    public GameObject splashPrefab;
    public GameObject shipHitPrefab;

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
        if (Time.time < spawnTime + 0.2f) return; 

        float currentWaveHeight = Mathf.Sin((transform.position.x + Time.time * waveSpeed) * waveFrequency) * waveAmplitude;

        if (transform.position.y <= currentWaveHeight)
        {
            Explode(splashPrefab);
        }
    }

    
    void OnCollisionEnter(Collision collision)
    {
        if (hasSplashed) return; 

        if (Time.time < spawnTime + 0.1f) return;

        
        if (collision.gameObject.GetComponent<CannonballImpact>() != null)
        {
            return;
        }

        bool hitAShip = false;



        
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(25); 
            hitAShip = true;
        }

        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(25); 
            hitAShip = true;
        }

        if (hitAShip)
        {
            Explode(shipHitPrefab);
        }
        else
        {
            Explode(splashPrefab);
        }
    }

    
    void Explode(GameObject prefab)
    {
        hasSplashed = true;

        if (prefab != null)
        {
            GameObject spawnedPrefab = Instantiate(prefab, transform.position, Quaternion.identity);
            Destroy(spawnedPrefab, 3f);
        }

        Destroy(gameObject);
    }
}