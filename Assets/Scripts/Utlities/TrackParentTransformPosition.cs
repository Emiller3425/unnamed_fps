using UnityEngine;

public class TrackParentTransformPosition : MonoBehaviour
{
    void Start()
    {
        transform.position = transform.parent.position;
    }
    void Update()
    {
        transform.position = transform.parent.position;
    }
}