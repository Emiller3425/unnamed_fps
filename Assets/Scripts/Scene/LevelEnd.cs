using UnityEngine;
public class LevelEnd : PlayerCubeZoneDetector
{
    protected override void OnTriggerEnter(Collider c)
    {
        base.OnTriggerEnter(c);
        if (isPlayer)
        {
          GameEvents.current.LevelEnd();  
        }
    }
}