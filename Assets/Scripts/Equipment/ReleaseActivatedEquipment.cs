using System.Collections;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class TimedFuseEquipment : Equipment
{
    protected float fuseTimer;
    protected override void Start()
    {
        base.Start();
        StartCoroutine(StartFuse());
    }

    protected IEnumerator StartFuse()
    {
        float currentFuseTimer = fuseTimer;
        while (currentFuseTimer > 0f)
        {
            currentFuseTimer -= Time.deltaTime;
            yield return null;
        }
        Detonate();
    }
}