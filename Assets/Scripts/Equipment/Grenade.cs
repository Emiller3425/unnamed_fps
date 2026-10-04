using Unity.VisualScripting;
using UnityEngine;
using UnityEditor.ShaderGraph.Internal;

public class Grenade : TimedFuseEquipment
{
    protected override void Start()
    {
        fuseTimer = 3.5f;
        damage = 100f;
        areaOfEffect = 5f;
        dentonateForce = 400f;
        base.Start();
    }

    public override void Detonate()
    {
        base.Detonate();
    }
    protected override void PlaySFX()
    {
        GameEvents.current.PlayVFX("grenadeExplosion", transform.position, Vector3.zero, Vector3.zero, null);
        GameEvents.current.PlaySFX("explosion");
    }
}