using UnityEngine;

public class PlayerDeathAnimator : MonoBehaviour
{
    public void TurnOffGravity()
    {
        GetComponentInParent<Rigidbody>().useGravity = false;
    }
}
