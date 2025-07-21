using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Músicas")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip endingMusic;

    private AudioSource audioSource;
    private bool isPlayingEndingMusic = false;

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
            isPlayingEndingMusic = false;
        }
    }

    public void PlayEndingMusic()
    {
        if (!isPlayingEndingMusic)
        {
            StartCoroutine(TransitionToEndingMusic());
        }
    }

    private IEnumerator TransitionToEndingMusic()
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime/2;
            yield return null;
        }

        audioSource.clip = endingMusic;

        yield return new WaitForSeconds(10f);
        audioSource.Play();
        isPlayingEndingMusic = true;

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime;
            yield return null;
        }
    }
}