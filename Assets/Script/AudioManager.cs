using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

    public static AudioManager audioManager;

    [Header ("---Audio SOurce---")]
    public AudioSource musicSource;
    public AudioSource SFXSource;


    [Header ("---AudioClip---")]
    public AudioClip introBacksound;
    public AudioClip inGameBacksound;
    public AudioClip battleBacksound;
    public AudioClip buttonClick;
    public AudioClip buttonHover;
    public AudioClip attackButtonSound;
    public AudioClip winFlashSound;
    public AudioClip walk;
    public AudioClip collectCoin;

    private void Awake()
    {
        if (audioManager == null)
        {
            audioManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "InGame" || SceneManager.GetActiveScene().name != "InGamePagi")
        {
            StartCoroutine(PlayIntroBacksound(3.5f)); // Memainkan introBacksound setelah 3.5 detik di scene pertama
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "InGame")
        {
            musicSource.clip = inGameBacksound;
            musicSource.Play();
        }
    }

    private IEnumerator PlayIntroBacksound(float waitTime) { 
        yield return new WaitForSeconds(waitTime);

        musicSource.clip = introBacksound;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    // gnti music
    public void ChangeMusic(AudioClip newClip, float fadeDuration)
    {
        if (musicSource.clip != newClip)
        {
            StartCoroutine(FadeOutAndChangeMusicCoroutine(newClip, fadeDuration));
        }
    }

    // fadeo Out
    public void FadeOutMusic(float fadeDuration)
    {
        StartCoroutine(FadeOutCoroutine(fadeDuration));
    }

    private IEnumerator FadeOutCoroutine(float fadeDuration)
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume; // Reset volume ke nilai awal
    }

    //
    private IEnumerator FadeOutAndChangeMusicCoroutine(AudioClip newClip, float fadeDuration)
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.Stop(); // Hentikan musik setelah fade out
        musicSource.volume = startVolume; // Reset volume ke nilai awal
        musicSource.clip = newClip; // Ganti musik ke yang baru
        musicSource.Play(); // Putar musik baru
    }
}


