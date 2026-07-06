
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyStatsManager : StatsManager
{
    protected Animator animator;
    protected void Start()
    {
        animator = GetComponent<Animator>();
    }
    public override void BulletDamage(float damage)
    {
        base.BulletDamage(damage);
        if (currentHealth <= 0f)
        {
            HandleDeath(5f);
        }
    }

    public override void ExplosiveDamage(float damage, Vector3 explosionOrigin=default, float explosionRadius=0f, float explosionForce=0f)
    {
        base.ExplosiveDamage(damage);
        if (currentHealth <= 0f)
        {
            HandleDeath(5f);
        }
    }

    protected override void HandleDeath(float timeToDestroy)
    {
        base.HandleDeath(timeToDestroy);
        animator.enabled = false;
        AddExperienceToPlayer();
    }

    protected void AddExperienceToPlayer()
    {
        PlayerStatsManager.Instance.ExperienceAdded(325);
    }
    protected override void OnDestroy()
    {
        // TODO: Enemy death -- Probably just use the ragdoll don't think we need animation
    }
}