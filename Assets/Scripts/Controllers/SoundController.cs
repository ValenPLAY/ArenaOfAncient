using UnityEngine;

public class SoundController : Singleton<SoundController>
{
    [SerializeField] SoundEffect soundEffectPrefab;
    [SerializeField] float pitchVariation;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SpawnSoundEffect(AudioClip incomingSound, Vector3 position)
    {
        SoundEffect spawnedSound = Instantiate(soundEffectPrefab, position, Quaternion.identity);
        spawnedSound.soundEffectSource.clip = incomingSound;
        spawnedSound.DefaultDuration = spawnedSound.soundEffectSource.clip.length;
        spawnedSound.soundEffectSource.PlayDelayed(Random.Range(0.0f, 0.1f));
    }
}
