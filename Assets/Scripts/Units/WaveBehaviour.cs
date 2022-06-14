using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Behaviour", menuName = "Wave Behaviours/Basic Wave Behaviour")]
public class WaveBehaviour : ScriptableObject
{
    [SerializeField] private List<Unit> waveUnitsToSpawn = new List<Unit>();

    public bool isScalingQuantity;
    public int defaultQuantity;

    public bool isSpecificWaveSpawn;
    public int specificWaveSpawn;

    public bool isSpawnEveryXRounds;

    public int minimumWaveToUseBehaviour;
    public int maximumWaveToUseBehaviour;

    public Unit GetUnitToSpawn()
    {
        if (waveUnitsToSpawn.Count == 0) return null;

        Unit unitToSpawn = waveUnitsToSpawn[Random.Range(0, waveUnitsToSpawn.Count)];
        return unitToSpawn;
    }
}
