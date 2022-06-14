using UnityEngine;

[CreateAssetMenu(fileName = "Effect Damage Area", menuName = "Ability Effects/Damage Area")]
public class AEDamageArea : AbilityEffect
{
    [SerializeField] HitZone hitZonePrefab;
    [SerializeField] float abilityDamage;
    [SerializeField] bool isSpawnedOnPlayer;
    //[SerializeField] float abilityRange;

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        if (isSpawnedOnPlayer)
        {
            SpawnController.Instance.CreateHitZone(hitZonePrefab, effectOwner, abilityDamage);
        }
        else
        {
            SpawnController.Instance.CreateHitZone(hitZonePrefab, effectOwner, GameController.Instance.playerWorldMousePos, abilityDamage);

        }
    }
}
