using UnityEngine;

public class getModelSize : MonoBehaviour
{
    void Start()
    {

        Renderer modelRenderer = GetComponent<Renderer>();
        if (modelRenderer != null)
        {
            Vector3 modelSize = modelRenderer.bounds.size;
            Debug.Log("Width: " + modelSize.x);
            Debug.Log("Height: " + modelSize.y);
            Debug.Log("Depth: " + modelSize.z);
        }
        
    }

    void Update()
    {
        
    }
}
