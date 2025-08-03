using UnityEngine;

public class ModelSizeChecker : MonoBehaviour
{
    void Start()
    {
        Bounds combinedBounds = new Bounds(transform.position, Vector3.zero);

        MeshRenderer renderers = GetComponent<MeshRenderer>();
        combinedBounds = renderers.bounds;
        //Debug.Log(gameObject.name + " Combined bounds center: " + combinedBounds.center);
        Debug.Log(gameObject.name + " Combined model size (world units): " + combinedBounds.size);

    }
}
