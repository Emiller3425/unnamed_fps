using UnityEngine;
public class PlayerCubeZoneDetector : MonoBehaviour
{
    protected bool isPlayer;
    protected virtual void OnTriggerEnter(Collider c)
    {
        if (c.GetComponentInParent<PlayerController>())
        {
            isPlayer = true;
        } else
        {
            isPlayer = false;
        }
    }
}