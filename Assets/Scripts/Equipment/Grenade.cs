using Unity.VisualScripting;
using UnityEngine;
using UnityEditor.ShaderGraph.Internal;

public class Grenade : TimedFuseEquipment
{
    protected override void Start()
    {
        Debug.Log("Grenade Spawned");
        fuseTimer = 3.5f;
        damage = 10f;
        areaOfEffect = 10f;
        base.Start();
    }
}