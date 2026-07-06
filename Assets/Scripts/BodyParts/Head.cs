using UnityEngine;

public class Head : BodyPart
{
    protected override void Start()
    {
        damageMultiplier = 1.5f;
        base.Start();
    }
}