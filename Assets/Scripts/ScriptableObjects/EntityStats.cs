using UnityEngine;

[CreateAssetMenu(fileName = "EntityStats", menuName = "ScriptableObjects/EntityStats")]
public class EntityStats : ScriptableObject
{
    [SerializeField] protected float health = 100f;
    [SerializeField] protected int pistolAmmo = 72;
    [SerializeField] protected int machineGunAmmo = 160;
    [SerializeField] protected int rifleAmmo = 180;

    public float GetHealth()
    {
        return health;
    }
    public int GetPistolAmmo()
    {
        return pistolAmmo;
    }
    public int GetMachineGunAmmo()
    {
        return machineGunAmmo;
    }
    public int GetRifleAmmo()
    {
        return rifleAmmo;
    }
}