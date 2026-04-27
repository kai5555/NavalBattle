using UnityEngine;
using UnityEngine.InputSystem; 

public class ShipCombat : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject leftAimCamera;
    public GameObject rightAimCamera;

    [Header("Weapons Setup")]
    public GameObject cannonballPrefab;
    public Transform[] leftCannons;
    public Transform[] rightCannons;

    [Header("Firing Stats")]
    public float fireForce = 3000f;
    public float upwardArc = 0.15f; 

    private bool isAimingLeft;
    private bool isAimingRight;

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        // 1. Read the Mouse Buttons
        isAimingLeft = Mouse.current.leftButton.isPressed;
        isAimingRight = Mouse.current.rightButton.isPressed;

        // 2. Toggle the Cameras
        // Cinemachine automatically blends smoothly when a camera is turned on/off!
        if (leftAimCamera != null) leftAimCamera.SetActive(isAimingLeft);
        if (rightAimCamera != null) rightAimCamera.SetActive(isAimingRight);

        // 3. Fire the cannons!
        // Press Spacebar to fire, but ONLY if you are currently aiming a side.
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isAimingLeft)
            {
                FireCannons(leftCannons, -transform.right);
            }
            else if (isAimingRight)
            {
                FireCannons(rightCannons, transform.right);
            }
        }
    }

    void FireCannons(Transform[] spawnPoints, Vector3 direction)
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject ball = Instantiate(cannonballPrefab, spawnPoint.position, spawnPoint.rotation);
            
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 fireVector = direction + (Vector3.up * upwardArc);
                rb.AddForce(fireVector.normalized * fireForce);
            }
            
            Destroy(ball, 5f);
        }
    }
}