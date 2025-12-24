using UnityEngine;

public class VisualEffectFollower : MonoBehaviour
{
    private Transform targetTransform;

    public void SetTarget(Transform target)
    {
        if (target != null)
        {
            targetTransform = target;
        } else
        {
            enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (targetTransform != null)
        {
            transform.position = targetTransform.position;
        }
    }
}