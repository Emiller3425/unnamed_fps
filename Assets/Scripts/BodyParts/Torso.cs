using UnityEngine;

public class Torso : BodyPart
{
    protected override void Start()
    {
        damageMultiplier = 1.2f;
        base.Start();
    }
}