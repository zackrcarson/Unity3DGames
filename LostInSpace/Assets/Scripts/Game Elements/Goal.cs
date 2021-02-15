using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Config Parameters
    [SerializeField] Canvas winScreen = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<MusicPlayer>().PlayWinMusic();

            // TODO: Cool animation, airlock behind you closing shut, hear zombies being killed in the distance, then quiet for a few seconds.. Then Win Screen pops up and win music FADES in.
            // TODO: Other Blocks: block shoot, zoom, swap, crouch, stab, flashlight locked to on, etc.
        }
    }
}
