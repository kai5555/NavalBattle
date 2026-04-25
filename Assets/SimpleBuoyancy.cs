using UnityEngine;

public class SimpleBuoyancy : MonoBehaviour
{
    [Header("Wave Settings (Match your Shader!)")]
    public float waveAmplitude = 1.5f;
    public float waveFrequency = 2.0f;
    public float waveSpeed = 1.0f;

    // We use an offset so the ship doesn't sit with its deck underwater
    public float floatHeightOffset = 0f; 

    void Update()
    {
        // 1. Get the ship's current position
        Vector3 pos = transform.position;
        
        // 2. Recreate the exact math from your Shader Graph!
        // Height = Sin((X + Time * Speed) * Frequency) * Amplitude
        float waveHeight = Mathf.Sin((pos.x + Time.time * waveSpeed) * waveFrequency) * waveAmplitude;
        
        // 3. Apply the new wave height to the ship, plus our offset
        transform.position = new Vector3(pos.x, waveHeight + floatHeightOffset, pos.z);
    }
}