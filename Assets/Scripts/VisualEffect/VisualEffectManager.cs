using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.VFX;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class VisualEffectManager : MonoBehaviour
{
    public VisualEffectItem[] visualEffects;
    private Dictionary<string, VisualEffectItem> effectLookup;

    private void Awake()
    {
        effectLookup = new Dictionary<string, VisualEffectItem>();
        foreach (VisualEffectItem item in visualEffects)
        {
            effectLookup.Add(item.name, item);
        }
    }

    private void Start()
    {
        GameEvents.current.OnPlayVFX += Play;
    }

    private void Play(string name, Vector3 position, Vector3 rotation, Vector3 velocity, Transform sourceToFollow)
    {
        if (effectLookup.TryGetValue(name, out VisualEffectItem effectItem))
        {
            if (sourceToFollow != null)
            {
                position = sourceToFollow.position;
            }

            GameObject vfxObject = Instantiate(effectItem.prefab, position, Quaternion.Euler(rotation));

            if (vfxObject.TryGetComponent<VisualEffect>(out var vfx))
            if (velocity != Vector3.zero)
                {
                    vfx.SetVector3("Velocity", velocity);
                }
                
            vfx.Play();
            Destroy(vfxObject, vfx.GetFloat("EffectMaxDuration"));

            FollowSource(vfxObject, sourceToFollow, vfx.GetFloat("EffectMaxDuration"));
        }
    }

    private void FollowSource(GameObject vfxObject, Transform sourceToFollow, float duration)
    {
        if (sourceToFollow != null)
        {
            
            if (vfxObject.TryGetComponent<VisualEffectFollower>(out var follower) == false)
            {
                follower = vfxObject.AddComponent<VisualEffectFollower>();
            }

            follower.SetTarget(sourceToFollow);

            Destroy(vfxObject, duration);
        }
        Destroy(vfxObject, duration);
    }

    private void OnDestroy()
    {
        GameEvents.current.OnPlayVFX -= Play;
    }
}