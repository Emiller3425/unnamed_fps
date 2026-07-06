using UnityEngine;

public class Limb : BodyPart
{
    protected override void Start()
    {
        damageMultiplier = 1.0f;
        base.Start();
    }
}