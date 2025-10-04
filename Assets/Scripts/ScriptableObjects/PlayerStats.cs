using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] private float health = 100f;
    [SerializeField] private int pistolAmmo = 72;
    [SerializeField] private int machineGunAmmo = 160;
    [SerializeField] private int rifleAmmo = 180;

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