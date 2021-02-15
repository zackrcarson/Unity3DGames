using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    // Config Parameters
    [SerializeField] float timeToPlayIntroMusic = 3f;
    [SerializeField] float fadeTime = 4f;
    [SerializeField] float introSongVolume = 0.5f;
    [SerializeField] float playSongVolume = 0.2f;
    [SerializeField] float LoseSongVolume = 1f;
    [SerializeField] float WinSongVolume = 1f;
    [SerializeField] AudioClip introSong = null;
    [SerializeField] AudioClip[] playSoundtrack = null;
    [SerializeField] AudioClip loseSong = null;
    [SerializeField] AudioClip winSong = null;

    // Cached References
    AudioSource audioSource = null;

    // State Variables
    bool playingPlayMusic = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        AudioListener.pause = false;

        audioSource.loop = true;
        audioSource.clip = introSong;
        audioSource.volume = introSongVolume;

        audioSource.Play();
    }

    private void Update()
    {
        if (!playingPlayMusic) { return; }

        if (!audioSource.isPlaying)
        {
            audioSource.clip = RandomPlaySong();
            audioSource.Play();
        }
    }

    public void StartCountDownToFadeOut()
    {
        StartCoroutine(FadeOutCountDown());
    }

    private IEnumerator FadeOutCountDown()
    {
        yield return new WaitForSeconds(timeToPlayIntroMusic);

        StartCoroutine(FadeOutAndIn());
    }

    private IEnumerator FadeOutAndIn()
    {
        while (audioSource.volume > 0f)
        {
            audioSource.volume -= introSongVolume * Time.deltaTime / fadeTime;

            yield return null;
        }
        audioSource.volume = 0f;

        audioSource.Stop();

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        audioSource.volume = 0f;
        audioSource.loop = false;
        audioSource.clip = RandomPlaySong();

        audioSource.Play();

        playingPlayMusic = true;

        while (audioSource.volume < playSongVolume)
        {
            audioSource.volume += playSongVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.volume = playSongVolume;
    }

    private IEnumerator FadeInWinMusic()
    {
        audioSource.volume = 0f;
        audioSource.loop = true;
        audioSource.clip = winSong;

        audioSource.Play();

        while (audioSource.volume < playSongVolume)
        {
            audioSource.volume += playSongVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.volume = playSongVolume;
    }

    public void PlayWinMusic()
    {
        playingPlayMusic = false;
        StopAllCoroutines();

        if (!audioSource) { audioSource = GetComponent<AudioSource>(); }

        //audioSource.loop = true;
        //audioSource.volume = WinSongVolume;
        //audioSource.clip = winSong;
        //audioSource.Play();
        StartCoroutine(FadeInWinMusic());
    }

    public void PlayDeathMusic()
    {
        playingPlayMusic = false;
        StopAllCoroutines();

        if (!audioSource) { audioSource = GetComponent<AudioSource>(); }

        audioSource.loop = true;
        audioSource.volume = LoseSongVolume;
        audioSource.clip = loseSong;
        audioSource.Play();
    }

    public void PlayPlayMusic()
    {
        if (!audioSource) { audioSource = GetComponent<AudioSource>(); }

        StartCountDownToFadeOut();
    }

    private AudioClip RandomPlaySong()
    {
        if (playSoundtrack.Length > 0)
        {
            int randomClipIndex = Random.Range(0, playSoundtrack.Length);

            return playSoundtrack[randomClipIndex];
        }
        else
        {
            return null;
        }
    }
}