using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public Transform player;

    public float spawnInterval = 45f; // Spawns a ship every 45 seconds
    public float spawnRadius = 200f;  // How far away they spawn (so they don't pop in on top of you!)
    public int maxEnemies = 3;        // Prevents your computer from crashing with 100 ships

    private float timer;

    void Start()
    {
        timer = spawnInterval; 
    }

    void Update()
    {
        // Safety check to ensure the player is still alive
        if (player == null || enemyPrefab == null) return;

        // Find out how many enemies are currently hunting you
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
        // 1. Pick a random angle in a full 360-degree circle around the player
        float randomAngle = Random.Range(0f, 360f);
        Vector3 spawnDirection = new Vector3(Mathf.Sin(randomAngle), 0f, Mathf.Cos(randomAngle));

        // 2. Multiply by the radius to push them far out into the fog
        Vector3 spawnPos = player.position + (spawnDirection * spawnRadius);

        // 3. Force them to spawn exactly at sea level
        spawnPos.y = 0f;

        // 4. Spawn the ship!
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        PopupManager.Instance.ShowPopup("WARNING: Pirate ship spotted!");
    }
}