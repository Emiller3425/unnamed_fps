
public class EnemyStatsManager : StatsManager
{
    public override void HealthSubtracted(float damage)
    {
        base.HealthSubtracted(damage);
        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
            PlayerStatsManager.Instance.ExperienceAdded(100);
        }
    }
    protected override void OnDestroy()
    {
        // TODO: Enemy death -- ideally a death animation
    }
}