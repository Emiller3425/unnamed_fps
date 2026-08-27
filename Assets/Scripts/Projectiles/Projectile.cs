using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float velocity;
    protected float projectileDamage;
    protected bool hasCollided = false;
    protected BoxCollider boxCollider;
    protected Rigidbody rigidBody;
    public void setDamage(float damage)
    {
        projectileDamage = damage;
    }
    protected void Start()
    {
        Destroy(gameObject, 3f);
    }

    protected void FixedUpdate()
    {
        if (velocity > 0)
        {
            transform.position += Time.deltaTime * velocity * transform.forward;
        }
    }

    protected void OnCollisionEnter(Collision c)
    {
        // We should not collide with another projectile
        if (c.gameObject.GetComponent<IProjectile>() != null)
        {
            return;
        }
        IDamageable damageableObject = c.gameObject.GetComponent<IDamageable>();
        if (damageableObject != null && !hasCollided)
        {
            if (!c.gameObject.GetComponentInParent<StatsManager>().isDead) {
                damageableObject.BulletDamage(projectileDamage, c.transform.position);

                // VFX
                ContactPoint contact = c.GetContact(0);
                Vector3 hitPoint = contact.point;
                Vector3 hitNormal = contact.normal;

                GameEvents.current.PlayVFX("bloodSplatter", hitPoint, Vector3.zero, hitNormal * 2, null);
            }
            Destroy(gameObject);
        }
    }
}