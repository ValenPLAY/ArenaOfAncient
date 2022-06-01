using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    private float defaultDuration = 5.0f;
    public float DefaultDuration
    {
        get => defaultDuration;
        set
        {
            defaultDuration = (value * soundEffectSource.pitch) + delayCompensation;
            currentDuration = defaultDuration;
        }
    }
    private float delayCompensation = 1.0f;
    private float currentDuration;
    public AudioSource soundEffectSource;
    // Start is called before the first frame update
    void Awake()
    {
        currentDuration = DefaultDuration;
        soundEffectSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDuration > 0)
        {
            currentDuration -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
