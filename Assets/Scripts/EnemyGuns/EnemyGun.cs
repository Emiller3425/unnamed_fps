using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class EnemyGun : MonoBehaviour
{
    public EntityStats entityStats;
    public AnimatorOverrideController weaponAnimationOverride;
    public GameObject muzzleFlashLight;
    public int damage;
    public GameObject bulletProjectile;
    protected BoxCollider boxCollider;
    protected GameObject gripAnchor;
    protected Transform muzzleTransform;
    protected RangedEnemyController parent;
  
    protected void Start()
    {
        muzzleTransform = transform.Find("Muzzle");
        parent = GetComponentInParent<RangedEnemyController>();
        if (parent)
        {
            parent.OnShoot += Shoot;
            parent.OnDeath += DropWeapon;
        }
    }

    protected void Shoot()
    {
        GameObject bullet = Instantiate(bulletProjectile, muzzleTransform.position, muzzleTransform.rotation);

        if (bullet.TryGetComponent<Projectile>(out var projectile)) {
            projectile.setDamage(damage);

            GameEvents.current.PlaySFX("gunshot");
            GameEvents.current.PlayVFX("glockMuzzleFlash", muzzleTransform.position, muzzleTransform.rotation.eulerAngles, Vector3.zero, muzzleTransform);
            GameEvents.current.SpawnLight(muzzleFlashLight, muzzleTransform.position, muzzleTransform.rotation.eulerAngles, 0.05f);
        }
    }
    protected void DropWeapon()
    {
        transform.SetParent(null, true);

        GetComponent<BoxCollider>().enabled = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(
            new Vector3(
                Random.Range(-50f, 50f),
                Random.Range(-50f, 50f),
                Random.Range(-50f, 50f)
            ));

        Destroy(gameObject, 3f);
    }
    protected void OnDisable()
    {
        if (parent) 
        {
            parent.OnShoot -= Shoot;
            parent.OnDeath -= DropWeapon;
        }
    }
}