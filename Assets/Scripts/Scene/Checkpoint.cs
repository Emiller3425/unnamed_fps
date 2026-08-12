using UnityEngine;

public class Checkpoint : PlayerCubeZoneDetector
{
    protected override void OnTriggerEnter(Collider c)
    {
        base.OnTriggerEnter(c);
        if (isPlayer)
        {
            GameEvents.current.UpdateRespawnPosition(transform.position);
        }
    }
}