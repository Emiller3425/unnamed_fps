using UnityEngine;

public class Torso : BodyPart
{
    protected override void Start()
    {
        damageMultiplier = 1.0f;
        base.Start();
    }
}