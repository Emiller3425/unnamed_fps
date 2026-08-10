using UnityEngine;

public class Checkpoint : CubeZoneDetector
{
    protected override void OnTriggerEnter(Collider c)
    {
        base.OnTriggerEnter(c);
        GameEvents.current.UpdateRespawnPosition(transform.position);
    }
}