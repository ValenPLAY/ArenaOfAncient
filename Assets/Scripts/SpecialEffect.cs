using UnityEngine;

public class SpecialEffect : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] bool isRandomScale;
    [SerializeField] bool isRandomScaleIndividual;
    [SerializeField] float randomScaleValue;
    [SerializeField] bool isRandomRotation;
    private Vector3 randomRotation;
    private Vector3 randomScale;
    private float zeroScaleCompensation = 0.01f;

    [Header("Sound Settings")]
    [SerializeField] AudioClip onSpawnSound;

    [Header("Destroy Values")]
    [SerializeField] bool isAutoDestroy = true;
    [SerializeField] float timeTillDestroyDefault = 5.0f;
    private float timeTillDestroyCurrent;

    private void Awake()
    {
        if (isRandomRotation)
        {
            randomRotation.y = Random.Range(0.0f, 360.0f);
            transform.rotation = Quaternion.Euler(randomRotation);
        }

        if (isRandomScaleIndividual && isRandomScale)
        {
            randomScale.x = (transform.localScale.x * Random.Range(1 - randomScaleValue, 1 + randomScaleValue)) + zeroScaleCompensation;
            randomScale.y = (transform.localScale.y * Random.Range(1 - randomScaleValue, 1 + randomScaleValue)) + zeroScaleCompensation;
            randomScale.z = (transform.localScale.z * Random.Range(1 - randomScaleValue, 1 + randomScaleValue)) + zeroScaleCompensation;
            transform.localScale = randomScale;
        }

        if (isRandomScale)
        {
            //randomScale = transform.localScale * Random.Range(1 - randomScaleValue, 1 + randomScaleValue);
            transform.localScale *= Random.Range(1 - randomScaleValue, 1 + randomScaleValue);
        }

        if (onSpawnSound != null)
        {
            SoundController.Instance.SpawnSoundEffect(onSpawnSound, transform.position);
        }

        timeTillDestroyCurrent = timeTillDestroyDefault;
    }

    private void Update()
    {
        if (isAutoDestroy)
        {
            if (timeTillDestroyCurrent <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                timeTillDestroyCurrent -= Time.deltaTime;
            }
        }

    }
}
