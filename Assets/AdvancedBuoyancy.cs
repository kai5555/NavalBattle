using UnityEngine;

public class AdvancedBuoyancy : MonoBehaviour
{
    [Header("Wave Settings (Match your Shader!)")]
    public float waveAmplitude = 1f;
    public float waveFrequency = 1.5f;
    public float waveSpeed = 1.5f;

    [Header("Ship Dimensions & Weight")]
    public float shipLength = 5f; 
    public float shipWidth = 2f;  
    public float floatHeightOffset = -1.4f;
    
    [Range(0.1f, 1.0f)]
    [Tooltip("Lower number = heavier ship that rocks less")]
    public float waveResistance = 0.4f; 
    
    [Tooltip("Lower number = slower, heavier rocking motion")]
    public float rockingSluggishness = 2.0f; 

    float GetWaveHeight(float xPosition)
    {
        return Mathf.Sin((xPosition + Time.time * waveSpeed) * waveFrequency) * waveAmplitude;
    }

    void Update()
    {
        Vector3 frontPos = transform.position + (transform.forward * shipLength);
        Vector3 backPos = transform.position - (transform.forward * shipLength);
        Vector3 rightPos = transform.position + (transform.right * shipWidth);
        Vector3 leftPos = transform.position - (transform.right * shipWidth);

        float frontHeight = GetWaveHeight(frontPos.x);
        float backHeight = GetWaveHeight(backPos.x);
        float rightHeight = GetWaveHeight(rightPos.x);
        float leftHeight = GetWaveHeight(leftPos.x);

        float averageHeight = (frontHeight + backHeight + rightHeight + leftHeight) / 4f;

        // Multiply by waveResistance to reduce the extreme angles!
        float pitchAngle = Mathf.Atan2(frontHeight - backHeight, shipLength * 2) * Mathf.Rad2Deg * waveResistance;
        float rollAngle = Mathf.Atan2(leftHeight - rightHeight, shipWidth * 2) * Mathf.Rad2Deg * waveResistance;

        transform.position = new Vector3(transform.position.x, averageHeight + floatHeightOffset, transform.position.z);
        
        Quaternion targetRotation = Quaternion.Euler(pitchAngle, transform.rotation.eulerAngles.y, rollAngle);
        // Use rockingSluggishness to make the ship react slower to the waves
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rockingSluggishness);
    }
}