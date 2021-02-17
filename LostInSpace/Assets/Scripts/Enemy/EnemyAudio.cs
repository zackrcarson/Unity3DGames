using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    // Config Parameters
    [SerializeField] float idleSoundVolume = 1f;
    [SerializeField] float attackSoundVolume = 1f;
    [SerializeField] float victorySoundVolume = 1f;
    [SerializeField] float deathSoundVolume = 1f;
    [SerializeField] float hurtSoundVolume = 1f;
    [SerializeField] AudioClip idleSound = null;
    [SerializeField] AudioClip attackSound = null;
    [SerializeField] AudioClip victorySound = null;
    [SerializeField] AudioClip deathSound = null;
    [SerializeField] AudioClip hurtSound = null;

    // Cached References
    AudioSource audioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayIdleSound()
    {
        audioSource.loop = true;
        audioSource.clip = idleSound;
        audioSource.volume = idleSoundVolume;
        audioSource.Play();
    }

    public void PlayAttackSound()
    {
        audioSource.loop = true;
        audioSource.clip = attackSound;
        audioSource.volume = attackSoundVolume;
        audioSource.Play();
    }

    public void PlayVictorySound()
    {
        audioSource.loop = true;
        audioSource.clip = victorySound;
        audioSource.volume = victorySoundVolume;
        audioSource.Play();
    }

    public void PlayDeathSound()
    {
        audioSource.loop = false;
        audioSource.clip = deathSound;
        audioSource.volume = deathSoundVolume;
        audioSource.Play();
    }

    public void PlayHurtSound()
    {
        audioSource.PlayOneShot(hurtSound, hurtSoundVolume);
    }
}
