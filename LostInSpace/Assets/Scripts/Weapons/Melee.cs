using UnityEngine;

public class Melee : MonoBehaviour
{
    // Config Parameters
    [SerializeField] BoxCollider boxCollider = null;
    [SerializeField] float swingAudioVolume = 1f;
    [SerializeField] AudioClip swingAudio = null;

    // Cached References
    Animator animator = null;
    AudioSource audioSource = null;

    // State Variables
    bool isStabbing = false;
    bool canStab = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        boxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canStab) { return; }
        if (isStabbing) { return; }

        if (Input.GetButtonDown("Stab"))
        {
            animator.SetTrigger("stab");
            audioSource.PlayOneShot(swingAudio, swingAudioVolume);
        }
    }

    public void StartStab()
    {
        boxCollider.enabled = true;
        isStabbing = true;
    }

    public void StopStab()
    {
        boxCollider.enabled = false;
        isStabbing = false;
    }

    public void AllowStabbing()
    {
        canStab = true;
        StopStab();
    }

    public void DisallowStabbing()
    {
        canStab = false;
    }

    public bool GetStabbing()
    {
        return isStabbing;
    }
}
