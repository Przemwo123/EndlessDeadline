using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public delegate void NewCeckpointEnter(GameObject checkpoint);
    public static event NewCeckpointEnter NewCeckpoint;

    private bool isCheckpointNow;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && NewCeckpoint != null && !isCheckpointNow)
        {
            NewCeckpoint(this.gameObject);
            GetComponent<SpriteRenderer>().color = Color.green;
            isCheckpointNow = true;
        }
    }

    private void ResetCheckpoint()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        isCheckpointNow = false;
    }
}