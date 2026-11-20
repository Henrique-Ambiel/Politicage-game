
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public static GameAudio Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;

    [Header("Button Sounds")]
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip gameplayButtonClick;

    [Header("Ending Sounds")]
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip defeatSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // =========================
    // MÚSICAS
    // =========================

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void PlayGameplayMusic()
    {
        PlayMusic(gameplayMusic);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null)
            return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    // =========================
    // BOTÕES
    // =========================

    // Som de urna: menu, opções, tutorial e créditos.
    public void PlayClick()
    {
        PlaySFX(buttonClick);
    }

    // Novo som: botões de decisão durante as crises.
    public void PlayGameplayClick()
    {
        PlaySFX(gameplayButtonClick);
    }

    // =========================
    // FINAIS
    // =========================

    public void PlayVictory()
    {
        PlaySFX(victorySound);
    }

    public void PlayDefeat()
    {
        PlaySFX(defeatSound);
    }

    // =========================
    // EFEITOS SONOROS
    // =========================

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    // =========================
    // VOLUME
    // =========================

    public void SetVolume(float volume)
    {
        AudioListener.volume = Mathf.Clamp01(volume);
    }
}
