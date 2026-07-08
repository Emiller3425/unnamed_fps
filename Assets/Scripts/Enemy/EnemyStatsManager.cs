
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyStatsManager : StatsManager
{
    protected Animator animator;
    protected void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }
    public override void BulletDamage(float damage, Vector3 hitNormal)
    {
        base.BulletDamage(damage, hitNormal);
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
}