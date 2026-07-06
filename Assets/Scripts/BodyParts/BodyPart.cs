using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BodyPart : MonoBehaviour, IDamageable
{
    protected float damageMultiplier;
    protected StatsManager statsManager;
    protected Rigidbody rb;
    protected Collider bodyCollider;
    protected int parentInstanceId;
    protected virtual void OnEnable()
    {
        GameEvents.current.OnEntityDeath += DisableRigidBody;
    }
    protected virtual void Start()
    {
        statsManager = GetComponentInParent<StatsManager>();
        rb = GetComponent<Rigidbody>();
        bodyCollider = GetComponent<Collider>();
        parentInstanceId = statsManager.gameObject.GetInstanceID();
    }

    protected virtual void Update()
    {

    }
    public virtual bool isDead => statsManager != null && statsManager.isDead;
    public virtual void BulletDamage(float damage)
    {
        if (statsManager)
        {
            statsManager.BulletDamage(damage * damageMultiplier);
        }
        else
        {
            Debug.LogError($"No Stats Manager found in {gameObject.name} parent");
        }
    }

    public virtual void ExplosiveDamage(float damage, Vector3 explosionOrigin, float explosionRadius, float explosionForce)
    {
        if (statsManager)
        {
            statsManager.ExplosiveDamage(damage * damageMultiplier);
        }
        else
        {
            Debug.LogError($"No Stats Manager found in {gameObject.name} parent");
        }

        Vector3 closestPoint = GetComponent<Collider>().ClosestPoint(explosionOrigin);
        Vector3 direction = (closestPoint - explosionOrigin).normalized;
        GameEvents.current.PlayVFX("bloodSplatter", closestPoint, Vector3.zero, direction * 2, null);

        if (rb != null && !rb.isKinematic)
        {
            rb.AddExplosionForce(explosionForce, explosionOrigin, explosionRadius, 1.5f, ForceMode.Impulse);
        }
    }

    protected virtual void DisableRigidBody(int instanceId)
    {
        if (instanceId == parentInstanceId)
        {
            rb.isKinematic = false;
        }
    }

    protected virtual void OnDestroy()
    {
        
    }
}