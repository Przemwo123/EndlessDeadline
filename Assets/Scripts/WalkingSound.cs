using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingSound : MonoBehaviour
{
    public float walkinSoundLength = 0.1f;
    public AudioClip[] walkinSound;

    void Start()
    {
        StartCoroutine(WalkinSoundCoroutine());
    }

    IEnumerator WalkinSoundCoroutine()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        AudioSource audioSource = GetComponent<AudioSource>();

        while (true)
        {
            if (playerController.GetIsGrounded() && playerController.GetIsWalking())
            {
                audioSource.clip = walkinSound[Random.Range(0, walkinSound.Length)];
                audioSource.pitch = Random.Range(2.8f, 3f);
                audioSource.Play();
                float timeWalkingSound = audioSource.clip.length;
                yield return new WaitForSeconds(timeWalkingSound);
            }
            else
            {
                yield return null;
            }
        }
    }
}