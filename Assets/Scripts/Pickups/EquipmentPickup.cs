public class EquipmentPickup : Pickup
{
    public override void HandlePickup()
    {
        PlayerStatsManager.Instance.EquipmentAdded(5);
        base.HandlePickup();
    }
}