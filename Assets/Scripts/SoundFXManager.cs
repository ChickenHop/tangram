using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [SerializeField] private AudioSource soundFXObject;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    
    public void playSoundFX(AudioClip clip, Transform transform, float volume)
    {
        AudioSource source = Instantiate(soundFXObject, transform.position, Quaternion.identity);
        source.clip = clip;
        source.volume = volume;
        source.Play();
        float clipLen = source.clip.length;
        Destroy(source.gameObject, clipLen);
    }
}
