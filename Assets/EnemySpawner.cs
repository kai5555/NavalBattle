using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public Transform player;

    public float spawnInterval = 45f;
    public float spawnRadius = 200f;
    public int maxEnemies = 3; 

    private float timer;

    void Start()
    {
        timer = spawnInterval; 
    }

    void Update()
    {
        if (player == null || enemyPrefab == null) return;

        // Find out how many enemies are currently hunting player
        GameObject[] currentEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Only spawn more if we haven't hit the maximum
        if (currentEnemies.Length < maxEnemies)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                SpawnEnemy();
                timer = spawnInterval; // Reset the clock!
            }
        }
    }

    void SpawnEnemy()
    {
        // Pick random angle in a full 360-degree circle around player
        float randomAngle = Random.Range(0f, 360f);
        Vector3 spawnDirection = new Vector3(Mathf.Sin(randomAngle), 0f, Mathf.Cos(randomAngle));

        // Multiply by radius to spawn them farther
        Vector3 spawnPos = player.position + (spawnDirection * spawnRadius);

        // Spawn at sea level
        spawnPos.y = 0f;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        PopupManager.Instance.ShowPopup("WARNING: Pirate ship spotted!");
    }
}