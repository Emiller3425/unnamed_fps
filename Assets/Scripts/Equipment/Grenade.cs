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
        base.Start();
    }
}