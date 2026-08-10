using UnityEngine;
public class CubeZoneDetector : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider c)
    {
        Debug.Log("Enter");
    }
}