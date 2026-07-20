using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip clickSFX;
    [SerializeField] private AudioClip collectSFX;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayClick()
    {
        if (clickSFX != null) sfxSource.PlayOneShot(clickSFX);
    }

    public void PlayCollect()
    {
        if (collectSFX != null) sfxSource.PlayOneShot(collectSFX);
    }
}