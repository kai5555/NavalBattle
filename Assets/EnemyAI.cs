using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Movement")]
    public Transform playerTarget;
    public float moveSpeed = 10f;
    public float turnSpeed = 25f;
    public float combatDistance = 40f; // How close it gets before turning its broadside

    [Header("AI Combat")]
    public GameObject cannonballPrefab;
    public Transform[] leftCannons;
    public Transform[] rightCannons;
    public float fireVelocity = 35f;
    public float fireCooldown = 4f;    // Seconds between volleys

    [Header("Audio")]
    public AudioClip cannonFireSound;

    private float nextFireTime = 0f;
    private AdvancedBuoyancy buoyancyScript;

    void Start()
    {
        // find the player
        ShipController player = FindAnyObjectByType<ShipController>();
        if (player != null) playerTarget = player.transform;

        // Grab buoyancy script, use this to know if dead
        buoyancyScript = GetComponent<AdvancedBuoyancy>();
    }

    void Update()
    {
        // If buoyancy is turned off, we are sinking. Stop driving and shooting.
        if (buoyancyScript != null && !buoyancyScript.enabled) return;
        
        // If there is no player to fight, just sit there.
        if (playerTarget == null) return;

        // Calculate distance and direction to the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);
        Vector3 directionToPlayer = (playerTarget.position - transform.position).normalized;

        Vector3 targetDirection;


        if (distanceToPlayer > combatDistance)
        {
            // CHASE MODE: Point the nose directly at the player
            targetDirection = directionToPlayer;
        }
        else
        {
            // BROADSIDE MODE: Use Vector Math to calculate a 90-degree angle from the player
            // makes AI try to sail in perfect circle around player
            targetDirection = Vector3.Cross(Vector3.up, directionToPlayer);
        }

        // Calculate how hard to turn the steering wheel to reach the target direction
        float angleToTarget = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);
        float steerInput = Mathf.Clamp(angleToTarget / 30f, -1f, 1f); 
        
        transform.Rotate(Vector3.up * steerInput * turnSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (Time.time >= nextFireTime)
        {
            CheckAndFireBroadside(directionToPlayer);
        }
    }

    void CheckAndFireBroadside(Vector3 directionToPlayer)
    {
        // Is player to the right? (in a 15-degree viewing cone)
        if (Vector3.Angle(transform.right, directionToPlayer) < 15f) 
        {
            FireCannons(rightCannons, transform.right);
            nextFireTime = Time.time + fireCooldown; // Reset the reload timer
        }
        // Is player to the Left?
        else if (Vector3.Angle(-transform.right, directionToPlayer) < 15f)
        {
            FireCannons(leftCannons, -transform.right);
            nextFireTime = Time.time + fireCooldown;
        }
    }

    void FireCannons(Transform[] spawnPoints, Vector3 fireDirection)
    {
        // Grab EVERY collider attached to this ship and its children
        Collider[] myColliders = GetComponentsInChildren<Collider>(); 

        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject ball = Instantiate(cannonballPrefab, spawnPoint.position, spawnPoint.rotation);
            if (cannonFireSound != null)
            {
                AudioSource.PlayClipAtPoint(cannonFireSound, spawnPoint.position);
            }
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            Collider ballCollider = ball.GetComponent<Collider>();

            // Loop through all the ship's parts and ignore them to not hit own ship
            if (ballCollider != null)
            {
                foreach (Collider shipPart in myColliders)
                {
                    Physics.IgnoreCollision(shipPart, ballCollider);
                }
            }
            
            if (rb != null)
            {
                Vector3 fireVector = (fireDirection + (Vector3.up * 0.15f)).normalized * fireVelocity;
                rb.linearVelocity = fireVector;
            }
            
            Destroy(ball, 5f);
        }
    }
}