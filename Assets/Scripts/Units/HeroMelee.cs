using UnityEngine;

public class HeroMelee : Hero
{
    [SerializeField] protected HitZone hitPrefab;

    protected override void Attack()
    {
        SpawnController.Instance.CreateHitZone(hitPrefab, this);

        base.Attack();
    }
}
