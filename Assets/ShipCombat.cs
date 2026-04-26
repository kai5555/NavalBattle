using UnityEngine;
using UnityEngine.InputSystem; 

public class ShipCombat : MonoBehaviour
{
    [Header("Weapons Setup")]
    public GameObject cannonballPrefab;
    public Transform[] leftCannons;
    public Transform[] rightCannons;

    [Header("Firing Stats")]
    public float fireForce = 3000f;
    public float upwardArc = 0.15f; // Gives the shot a slight parabolic arc

    void Update()
    {
        if (Keyboard.current == null) return;

        // Fire Left Broadside (Q Key)
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            FireCannons(leftCannons, -transform.right);
        }

        // Fire Right Broadside (E Key)
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            FireCannons(rightCannons, transform.right);
        }
    }

    void FireCannons(Transform[] spawnPoints, Vector3 direction)
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            // 1. Spawn a cannonball at each point
            GameObject ball = Instantiate(cannonballPrefab, spawnPoint.position, spawnPoint.rotation);
            
            // 2. Grab its physics component
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 3. Add explosive force! (Direction + a slight upward tilt)
                Vector3 fireVector = direction + (Vector3.up * upwardArc);
                rb.AddForce(fireVector.normalized * fireForce);
            }
            
            // 4. Destroy the ball after 5 seconds so it doesn't lag the game
            Destroy(ball, 5f);
        }
    }
}