using Unity.VisualScripting;
using UnityEngine;
using UnityEditor.ShaderGraph.Internal;

public class Grenade : TimedFuseEquipment
{
    protected override void Start()
    {
        fuseTimer = 3.5f;
        damage = 100f;
        areaOfEffect = 3f;
        dentonateForce = 50f;
        base.Start();
    }

    protected override void Detonate()
    {
        base.Detonate();
        GameEvents.current.PlaySFX("explosion");
    }
}