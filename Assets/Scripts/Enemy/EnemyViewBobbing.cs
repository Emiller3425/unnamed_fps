using UnityEngine;

public class EnemyViewBobbing : MonoBehaviour
{
    private Vector3 pivotPoint;
    void Start()
    {
        pivotPoint = transform.parent.position;
    }

    void Update()
    {
        pivotPoint = transform.parent.position;
        transform.position = pivotPoint;
    }
}