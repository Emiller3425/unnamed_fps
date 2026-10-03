using System;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticleCollision : MonoBehaviour
{
    // TODO: Make particles work with game events system
    private ParticleSystem bloodParticleSystem;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    [SerializeField] private GameObject bloodSplatterPrefab;

    private void Start()
    {
        bloodParticleSystem = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        _ = bloodParticleSystem.GetCollisionEvents(other, collisionEvents);

        Vector3 collisionPosition = collisionEvents[0].intersection;
        Vector3 collisionNormal = collisionEvents[0].normal;

        Quaternion rotation = Quaternion.LookRotation(-collisionNormal);

        GameObject bloodSplatter = Instantiate(bloodSplatterPrefab, collisionPosition, rotation);

        Destroy(bloodSplatter, 5f);
    }

}