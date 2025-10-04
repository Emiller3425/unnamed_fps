using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [SerializeField] private float health = 100f;
    [SerializeField] private int ammo = -1;

    public float GetHealth()
    {
        return health;
    }
    public int GetAmmo()
    {
        return ammo;
    }

}