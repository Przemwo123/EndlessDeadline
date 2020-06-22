using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodlogaToLawa : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                other.gameObject.transform.position = new Vector3(0, 0, 0);
                break;
            case "Enemy":
                Destroy(other.gameObject);
                break;
        }
    }
}
