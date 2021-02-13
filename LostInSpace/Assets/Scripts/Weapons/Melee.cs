using UnityEngine;

public class Melee : MonoBehaviour
{
    // Config Parameters
    [SerializeField] BoxCollider boxCollider = null;

    // Cached References
    Animator animator = null;

    // State Variables
    bool isStabbing = false;
    bool canStab = false;

    // Start is called before the first frame update
    void Start()
    {
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
