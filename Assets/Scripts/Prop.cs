using UnityEngine;

public class Prop : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] bool isRandomRotation;
    private Vector3 randomRotation;

    [Header("Size")]
    [SerializeField] bool isRandomSize;
    [SerializeField] float randomSizeModifier;
    private Vector3 randomScale;
    private float zeroScaleCompensation = 0.01f;

    // Start is called before the first frame update
    void Awake()
    {
        if (isRandomRotation)
        {
            randomRotation.y = Random.Range(0.0f, 360.0f);
            transform.rotation = Quaternion.Euler(randomRotation);
        }

        if (isRandomSize)
        {
            randomScale.x = (transform.localScale.x * Random.Range(1 - randomSizeModifier, 1 + randomSizeModifier)) + zeroScaleCompensation;
            randomScale.y = (transform.localScale.y * Random.Range(1 - randomSizeModifier, 1 + randomSizeModifier)) + zeroScaleCompensation;
            randomScale.z = (transform.localScale.z * Random.Range(1 - randomSizeModifier, 1 + randomSizeModifier)) + zeroScaleCompensation;
            transform.localScale = randomScale;
        }
    }
}
