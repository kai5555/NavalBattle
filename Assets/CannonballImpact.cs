using UnityEngine;

public class CannonballImpact : MonoBehaviour
{
    [Header("Impact Visuals")]
    public GameObject splashPrefab;

    [Header("Wave Settings (Match your Shader!)")]
    public float waveAmplitude = 1.5f;
    public float waveFrequency = 2.0f;
    public float waveSpeed = 1.0f;

    private bool hasSplashed = false; 
    
    // NEW: We will store the exact time the cannonball was fired
    private float spawnTime; 

    void Start()
    {
        // Record the time the moment this script wakes up
        spawnTime = Time.time;
    }

    void Update()
    {
        if (hasSplashed) return;

        // NEW: The "Arming Timer". Ignore the water for the first 0.2 seconds!
        if (Time.time < spawnTime + 0.2f) return;

        float currentWaveHeight = Mathf.Sin((transform.position.x + Time.time * waveSpeed) * waveFrequency) * waveAmplitude;

        if (transform.position.y <= currentWaveHeight)
        {
            hasSplashed = true;

            if (splashPrefab != null)
            {
                Instantiate(splashPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}