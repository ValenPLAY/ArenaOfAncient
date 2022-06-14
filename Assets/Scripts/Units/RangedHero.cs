using UnityEngine;

public class RangedHero : Hero
{
    [Header("Ranged Options")]
    [SerializeField] bool isInstant;
    [SerializeField] Projectile unitProjectile;
    [SerializeField] Transform firingPoint;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        if (firingPoint == null) firingPoint = upperBody.transform;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        base.Attack();
        if (unitAnimator != null)
        {
            unitAnimator.SetTrigger("Attack");
        }
        else
        {
            FireProjectile(GameController.Instance.playerWorldMousePos);
        }

    }

    protected virtual void FireProjectile(Vector3 targetPosition)
    {
        if (isInstant)
        {
            SpawnController.Instance.SpawnProjectile(targetPosition, this, unitProjectile, damageActual);
        }
        else
        {
            SpawnController.Instance.SpawnProjectile(firingPoint.position, targetPosition, this, unitProjectile, damageActual);
        }
    }
}
