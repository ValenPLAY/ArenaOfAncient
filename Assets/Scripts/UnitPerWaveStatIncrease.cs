using UnityEngine;

public class UnitPerWaveStatIncrease : MonoBehaviour
{
    Unit scalingUnit;
    float currentWave;
    [SerializeField] float damageIncreaseMultiplier;
    [SerializeField] float healthIncreaseMultiplier;
    [SerializeField] float movementSpeedIncreaseMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        scalingUnit = GetComponent<Unit>();
        currentWave = GameController.Instance.CurrentWave;
        ScaleUnit();
        Destroy(this);
    }

    void ScaleUnit()
    {
        if (scalingUnit == null) return;
        if (GameController.Instance == null) return;
        if (damageIncreaseMultiplier > 0) scalingUnit.Damage *= 1 + (currentWave * damageIncreaseMultiplier);
        if (healthIncreaseMultiplier > 0) scalingUnit.Health *= 1 + (currentWave * healthIncreaseMultiplier);
        if (movementSpeedIncreaseMultiplier > 0) scalingUnit.MovementSpeed *= 1 + (currentWave * movementSpeedIncreaseMultiplier);
    }
}
