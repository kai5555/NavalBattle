using UnityEngine;
using UnityEngine.InputSystem;

public class ShipCombat : MonoBehaviour
{
    [Header("Cameras & Aiming")]
    public GameObject leftAimCamera;
    public GameObject rightAimCamera;
    public LineRenderer trajectoryLine;
    public int trajectoryPoints = 30;
    public float timeBetweenPoints = 0.1f;

    [Header("Mouse Aiming Controls")]
    public float aimSensitivity = 0.005f; 
    public float maxElevation = 0.6f;  
    public float minElevation = 0.05f;  
    public float maxPan = 0.3f;    

    [Header("Weapons Setup")]
    public GameObject cannonballPrefab;
    public Transform[] leftCannons;
    public Transform[] rightCannons;

    [Header("Firing Stats")]
    public float fireVelocity = 30f;

    private bool isAimingLeft;
    private bool isAimingRight;

    private float currentElevation = 0.15f;
    private float currentPan = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        isAimingLeft = Mouse.current.leftButton.isPressed;
        isAimingRight = Mouse.current.rightButton.isPressed;

        if (leftAimCamera != null) leftAimCamera.SetActive(isAimingLeft);
        if (rightAimCamera != null) rightAimCamera.SetActive(isAimingRight);

        if (isAimingLeft || isAimingRight)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            currentElevation += mouseDelta.y * aimSensitivity;
            currentElevation = Mathf.Clamp(currentElevation, minElevation, maxElevation);

            float panDirection = isAimingLeft ? 1f : -1f;
            currentPan += mouseDelta.x * aimSensitivity * panDirection;
            currentPan = Mathf.Clamp(currentPan, -maxPan, maxPan);
        }
        else
        {
            currentPan = 0f; 
        }

        if (isAimingLeft)
        {
            trajectoryLine.enabled = true;
            Vector3 aimDir = -transform.right + (transform.forward * currentPan);
            DrawTrajectory(leftCannons[leftCannons.Length / 2], aimDir); 
        }
        else if (isAimingRight)
        {
            trajectoryLine.enabled = true;
            Vector3 aimDir = transform.right + (transform.forward * currentPan);
            DrawTrajectory(rightCannons[rightCannons.Length / 2], aimDir);
        }
        else
        {
            trajectoryLine.enabled = false;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isAimingLeft)
            {
                Vector3 aimDir = -transform.right + (transform.forward * currentPan);
                FireCannons(leftCannons, aimDir);
            }
            else if (isAimingRight)
            {
                Vector3 aimDir = transform.right + (transform.forward * currentPan);
                FireCannons(rightCannons, aimDir);
            }
        }
    }

    void DrawTrajectory(Transform startPoint, Vector3 direction)
    {
        trajectoryLine.positionCount = trajectoryPoints;
        
        Vector3 initialVelocity = (direction + (Vector3.up * currentElevation)).normalized * fireVelocity;
        
        Vector3 currentPosition = startPoint.position;
        Vector3 currentVelocity = initialVelocity;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            trajectoryLine.SetPosition(i, currentPosition);
            
            currentVelocity += Physics.gravity * timeBetweenPoints;
            currentPosition += currentVelocity * timeBetweenPoints;

            if (currentPosition.y < 0f) 
            {
                trajectoryLine.positionCount = i + 1;
                break;
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
                Vector3 fireVector = (direction + (Vector3.up * currentElevation)).normalized * fireVelocity;
                rb.linearVelocity = fireVector;
            }
            
            Destroy(ball, 5f);
        }
    }
}