using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    // Config Parameters
    [SerializeField] float timeToPlayIntroMusic = 3f;
    [SerializeField] float fadeTime = 4f;
    [SerializeField] float playSongVolume = 0.2f;
    [SerializeField] float LoseSongVolume = 1f;
    [SerializeField] float WinSongVolume = 1f;
    [SerializeField] AudioClip introSong = null;
    [SerializeField] AudioClip playSong = null;
    [SerializeField] AudioClip winSong = null;
    [SerializeField] AudioClip deathSong = null;

    // Cached References
    AudioSource audioSource = null;

    // State variables
    float musicVolume = 1f;

    private void Awake()
    {
        int numberMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;

        if (numberMusicPlayers == 1)
        {
            DontDestroyOnLoad(gameObject);

            int sceneNumber = SceneManager.GetActiveScene().buildIndex;

            audioSource = GetComponent<AudioSource>();
            musicVolume = audioSource.volume;
            audioSource.loop = true;

            if (sceneNumber == 0)
            {
                audioSource.clip = introSong;
            }
            else
            {
                audioSource.volume = playSongVolume;
                audioSource.clip = playSong;
            }

            audioSource.Play();

        }
        else
        {
            Destroy(gameObject);
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
            audioSource.volume -= musicVolume * Time.deltaTime / fadeTime;

            yield return null;
        }
        
        audioSource.Stop();
        audioSource.volume = musicVolume;

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        audioSource.volume = 0f;
        audioSource.clip = playSong;

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
        if (!audioSource) { audioSource = GetComponent<AudioSource>(); }

        audioSource.volume = WinSongVolume;
        audioSource.clip = winSong;
        audioSource.Play();
    }

    public void PlayDeathMusic()
    {
        if (!audioSource) { audioSource = GetComponent<AudioSource>(); }

        audioSource.volume = LoseSongVolume;
        audioSource.clip = deathSong;
        audioSource.Play();
    }

    public void PlayPlayMusic()
    {
        if (!audioSource) { audioSource = GetComponent<AudioSource>(); }

        audioSource.volume = playSongVolume;
        audioSource.clip = playSong;
        audioSource.Play();
    }

    //public void ChangeVolume(float volume)
    //{
    //    audioSource.volume = volume;
    //}

    //public float GetVolume()
    //{
    //    return audioSource.volume;
    //}
}
