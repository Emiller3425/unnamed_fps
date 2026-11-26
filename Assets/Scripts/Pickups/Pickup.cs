using UnityEngine;

public enum PickupType
{
    HEALTH,
    EQUIPMENT,
    PISTOLAMMO,
    SHOTGUNAMMO,
    RIFLEAMMO,
    SMGAMMO
}

public abstract class Pickup : MonoBehaviour, IPickupable
{
    protected Collider pickupCollider;
    public void Start()
    {
        pickupCollider = GetComponent<Collider>();
    }

    public virtual void HandlePickup()
    {
        Destroy(gameObject, 0f);
    }

    protected virtual void OnTriggerEnter(Collider triggerCollider)
    {
        if (triggerCollider.gameObject.GetComponent<PlayerController>())
        {
            HandlePickup();
        }
    }
}
