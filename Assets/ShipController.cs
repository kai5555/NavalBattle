using UnityEngine;
using UnityEngine.InputSystem; 

public class ShipController : MonoBehaviour
{
    [Header("Ship Handling")]
    public float topSpeed = 15f; 
    public float turnSpeed = 40f; 
    
    [Tooltip("Lower number means it takes longer to reach top speed")]
    public float acceleration = 1.5f; 

    private float currentSpeed = 0f;
    private float currentTurn = 0f;

    void Update()
    {
        float targetThrottle = 0f;
        float targetSteer = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) targetThrottle = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) targetThrottle = -1f;
            
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) targetSteer = 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) targetSteer = -1f;
        }

        // Smoothly interpolate current speed and turn based on target inputs
        currentSpeed = Mathf.Lerp(currentSpeed, targetThrottle * topSpeed, Time.deltaTime * acceleration);
        currentTurn = Mathf.Lerp(currentTurn, targetSteer * turnSpeed, Time.deltaTime * acceleration);

        // Apply the smoothed momentum
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        float speedPercentage = Mathf.Abs(currentSpeed) / topSpeed;
        transform.Rotate(Vector3.up * currentTurn * speedPercentage * Time.deltaTime);
    }
}