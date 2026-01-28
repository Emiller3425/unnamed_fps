using System.Buffers.Text;
using NUnit.Framework.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "GunStats", menuName = "ScriptableObjects/GunStats")]
public class GunStats : ScriptableObject
{
    [SerializeField] protected bool isPlayerGun = false;
    [SerializeField] protected int magSize = 30;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected int currentAmmo;
    [SerializeField] protected int currentMag;
    [SerializeField] protected float maxReloadBuffer;
    [SerializeField] protected float maxFireRateBuffer;
    [SerializeField] protected float maxRange;

    public bool GetIsPlayerGun()
    {
        return isPlayerGun;
    }
    public int GetMagSize()
    {
        return magSize;
    }
    public float GetDamage()
    {
        return damage;
    }
    public float GetCurrentAmmo()
    {
        return currentAmmo;
    }
    public int GetCurrentMag()
    {
        return currentMag;
    }
    public float GetMaxReloadBuffer()
    {
        return maxReloadBuffer;
    }
    public float GetMaxFireRateBuffer()
    {
        return maxFireRateBuffer;
    }
    public float GetMaxRange()
    {
        return maxRange;
    }
}