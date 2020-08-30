using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodlogaToLawa : MonoBehaviour
{
    public LevelManager levelManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                levelManager.MoveToCheckpoint();
                break;
            case "Enemy":
                Destroy(other.gameObject);
                break;
        }
    }
}
