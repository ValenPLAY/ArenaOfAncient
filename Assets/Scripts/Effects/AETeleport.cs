using UnityEngine;

[CreateAssetMenu(fileName = "Effect Teleport", menuName = "Ability Effects/Teleport")]

public class AETeleport : AbilityEffect
{
    public float teleportDistance;

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        CharacterController heroCharacterController = effectOwner.GetComponent<CharacterController>();
        Vector3 teleportVector = GameController.Instance.playerWorldMousePos - effectOwner.transform.position;
        Vector3.Normalize(teleportVector);

        if (teleportDistance == 0) teleportDistance = 1.0f;
        heroCharacterController.Move(teleportVector * teleportDistance);
    }
}
