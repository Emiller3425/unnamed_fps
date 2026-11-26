public class HealthPickup : Pickup
{
    public override void HandlePickup()
    {
        PlayerStatsManager.Instance.HealthAdded(100f);
        base.HandlePickup();
    }
}