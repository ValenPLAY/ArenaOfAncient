public class AESpawnProjectile : AbilityEffect
{
    public Projectile spawnedProjectilePrefab;
    public float projectileDamage;

    public override void ApplyEffect()
    {
        if (spawnedProjectilePrefab != null)
        {
            base.ApplyEffect();
            SpawnController.Instance.SpawnProjectile(effectOwner.transform.position, GameController.Instance.playerWorldMousePos, effectOwner, spawnedProjectilePrefab, projectileDamage);
        }
    }
}
