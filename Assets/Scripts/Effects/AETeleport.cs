using UnityEngine;

public class AETeleport : AbilityEffect
{
    public float teleportDistance;

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        CharacterController heroCharacterController = effectOwner.GetComponent<CharacterController>();
        Vector3 teleportVector = GameController.Instance.playerWorldMousePos - effectOwner.transform.position;

        //Vector3 teleportHeightMove = Vector3.zero;
        //teleportHeightMove.y = heroCharacterController.bounds.size.y;
        //heroCharacterController.Move(teleportHeightMove);

        Vector3.Normalize(teleportVector);
        heroCharacterController.Move(teleportVector * teleportDistance);
        //effectOwner.transform.position += teleportVector;
    }
}
