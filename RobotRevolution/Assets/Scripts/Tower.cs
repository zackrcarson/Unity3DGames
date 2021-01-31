using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // Config Parameters
    [SerializeField] Transform objectToPan = null;
    [SerializeField] Transform targetToFollow = null;

    // Update is called once per frame
    void Update()
    {
        objectToPan.LookAt(targetToFollow);
    }
}
