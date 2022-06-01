using UnityEngine;

public class BasicProjectile : Projectile
{
    protected override void OnEnable()
    {
        base.OnEnable();
        projectileRigidBody.AddForce(transform.forward * projectileSpeed);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        Unit hitUnit = other.gameObject.GetComponent<Unit>();

        if (hitUnit == null) return;
        if (hitUnit == projectileOwner) return;

        if (isDealDamageOnHit)
        {
            hitUnit.TakeDamage(projectileDamage);
        }

        if (isDestroyOnHit)
        {
            DestroyProjectile();
        }
    }
}
