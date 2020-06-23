using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAfterTime : MonoBehaviour
{
    public float time = 1;

    void Update()
    {
        Destroy(gameObject, time);
    }
}
