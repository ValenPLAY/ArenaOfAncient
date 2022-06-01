using UnityEngine;

public class SpawnController : Singleton<SpawnController>
{
    Vector3 checkPosition;
    Vector3 spawnPosition;
    float raycastHeight = 30.0f;

    Vector3 raycastFrom;

    public Unit SpawnEnemy(Unit unitToSpawn)
    {
        if (unitToSpawn == null) return null;

        spawnPosition = EnemyCheckPositionToSpawn();
        if (spawnPosition != Vector3.zero)
        {
            Unit spawnedUnit = Instantiate(unitToSpawn, spawnPosition, Quaternion.identity);
            GameController.Instance.enemiesOnMap.Add(spawnedUnit);
            return spawnedUnit;
        }
        else
        {
            return null;
        }

    }

    public Vector3 EnemyCheckPositionToSpawn()
    {
        Vector3 pointDifference = GameController.Instance.arena.pointTopRight.position - GameController.Instance.arena.pointBotLeft.position;
        checkPosition = GameController.Instance.arena.pointBotLeft.position;
        checkPosition.x += pointDifference.x * Random.Range(0.0f, 1.0f);
        checkPosition.z += pointDifference.z * Random.Range(0.0f, 1.0f);
        checkPosition.y += raycastHeight;

        Ray ray = new Ray(checkPosition, Vector3.down);
        Debug.DrawRay(checkPosition, Vector3.down * 100.0f, Color.white, 10f);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                return hit.point;
                //Debug.Log("Hit");
                //SpawnEnemy(GameController.Instance.waveEnemy, hit.point);
            }
            else
            {
                return Vector3.zero;
            }

        }
        else
        {
            return Vector3.zero;
        }
    }

    // Projectile Spawn
    public void SpawnProjectile(Vector3 startPosition, Vector3 endPosition, Unit owner, Projectile projectilePrefab, float damage)
    {
        Projectile firedProjectile = Instantiate(projectilePrefab, startPosition, Quaternion.identity);
        firedProjectile.transform.LookAt(endPosition);
        firedProjectile.projectileOwner = owner;
        firedProjectile.projectileDamage = damage;
        firedProjectile.gameObject.SetActive(true);
    }

    public void SpawnProjectile(Vector3 position, Unit owner, Projectile projectilePrefab, float damage)
    {
        Projectile firedProjectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        firedProjectile.projectileOwner = owner;
        firedProjectile.projectileDamage = damage;
        firedProjectile.gameObject.SetActive(true);
    }

    public void CreateHitZone(HitZone hitZonePrefab, Unit attackingUnit)
    {
        HitZone createdHitZone = Instantiate(hitZonePrefab, attackingUnit.transform);
        createdHitZone.hitZoneOwner = attackingUnit;
        createdHitZone.gameObject.SetActive(true);
    }

    public void CreateHitZone(HitZone hitZonePrefab, Unit castingUnit, float abilityDamage)
    {
        HitZone createdHitZone = Instantiate(hitZonePrefab, castingUnit.transform.position, Quaternion.identity);
        createdHitZone.hitZoneOwner = castingUnit;
        createdHitZone.hitZoneDamage = abilityDamage;
        createdHitZone.gameObject.SetActive(true);
    }

    public void CreateHitZone(HitZone hitZonePrefab, Unit castingUnit, Vector3 hitzoneSpawnPosition, float abilityDamage)
    {
        HitZone createdHitZone = Instantiate(hitZonePrefab, hitzoneSpawnPosition, Quaternion.identity);
        createdHitZone.hitZoneOwner = castingUnit;
        createdHitZone.hitZoneDamage = abilityDamage;
        createdHitZone.gameObject.SetActive(true);
    }

    public Buff CreateBuff(Unit buffingUnit, Buff incomingBuff)
    {
        Buff createdBuff = Instantiate(incomingBuff, buffingUnit.transform);
        buffingUnit.buffs.Add(createdBuff);
        return createdBuff;

    }

    public Modificator CreateModificator(Modificator spawningModificator, Unit affectedUnit)
    {
        Modificator createdModificator = Instantiate(spawningModificator, affectedUnit.transform);
        return createdModificator;
    }
}
