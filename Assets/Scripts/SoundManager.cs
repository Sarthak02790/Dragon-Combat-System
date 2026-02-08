using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Music & Voices")]
    public AudioClip battleMusic;
    public AudioClip fightVoice;
    public AudioClip gameOverVoice;

    [Header("Dragon SFX")]
    public AudioClip fireBreath;
    public AudioClip tailSlam;
    public AudioClip flyLaunch;

    private bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartBattleSounds();
    }

    public void StartBattleSounds()
    {
        isGameOver = false;

        // Start Music
        if (battleMusic != null)
        {
            musicSource.clip = battleMusic;
            musicSource.loop = true;
            musicSource.Play();
        }

        // Play "FIGHT!"
        if (fightVoice != null)
        {
            sfxSource.PlayOneShot(fightVoice);
        }
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            sfxSource.pitch = Random.Range(0.9f, 1.1f);
            sfxSource.PlayOneShot(clip, volume);
        }
    }

    // Call this when the Win Panel activates
    public void TriggerGameOver()
    {
        isGameOver = true;
        musicSource.Stop(); // Stop the battle music
        sfxSource.Stop();  // Stop any ongoing dragon roars/fire

        if (gameOverVoice != null)
        {
            // Play game over sound bypassing the isGameOver check
            sfxSource.pitch = 1f;
            sfxSource.PlayOneShot(gameOverVoice);
        }
    }
}