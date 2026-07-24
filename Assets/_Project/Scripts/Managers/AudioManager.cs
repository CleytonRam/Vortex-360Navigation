using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Clips")]
    [SerializeField] private AudioClip clickSFX;
    [SerializeField] private AudioClip collectSFX;
    [SerializeField] private AudioClip spawnSFX;
    [SerializeField] private AudioClip backgroundMusic;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayClick() { if (clickSFX != null) sfxSource.PlayOneShot(clickSFX); }
    public void PlayCollect() { if (collectSFX != null) sfxSource.PlayOneShot(collectSFX); }
    public void PlaySpawn() { if (spawnSFX != null) sfxSource.PlayOneShot(spawnSFX); }

    public void PauseMusic() { if (musicSource != null) musicSource.Pause(); }
    public void ResumeMusic() { if (musicSource != null) musicSource.UnPause(); }
}