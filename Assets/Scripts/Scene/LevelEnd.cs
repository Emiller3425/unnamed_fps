using UnityEngine;
public class LevelEnd : CubeZoneDetector
{
    protected override void OnTriggerEnter(Collider c)
    {
        base.OnTriggerEnter(c);
        GameEvents.current.LevelEnd();
    }
}