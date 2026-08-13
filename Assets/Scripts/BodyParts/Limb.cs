using UnityEngine;

public class Limb : BodyPart
{
    protected override void Start()
    {
        damageMultiplier = 0.75f;
        base.Start();
    }
}