using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Músicas")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    private AudioSource audioSource;
    private bool isPlayingGameMusic = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 0.5f;
    }

    private void Start()
    {
        PlayMenuMusic();
    }

    public void PlayMenuMusic()
    {
        if (audioSource.clip != menuMusic)
        {
            audioSource.clip = menuMusic;
            audioSource.Play();
            isPlayingGameMusic = false;
        }
    }

    public void PlayGameMusic()
    {
        if (!isPlayingGameMusic)
        {
            StartCoroutine(TransitionToGameMusic());
        }
    }

    private IEnumerator TransitionToGameMusic()
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime;
            yield return null;
        }

        audioSource.clip = gameMusic;
        audioSource.Play();
        isPlayingGameMusic = true;

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime;
            yield return null;
        }
    }
}