using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    public Transform player;
    private PlayerController playerController;
    private Rigidbody2D playerRigidbody2D;
    private Vector2 newCameraVector2;
    private Vector2 virtualVector2;
    private Vector2 playerVelocity;
    private float temp;
    private bool flipPlayer;

    private void Start()
    {
        if (player == null)
            player = GameObject.Find("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        playerRigidbody2D = player.GetComponent<Rigidbody2D>();
        newCameraVector2 = player.transform.position;
        transform.position = new Vector3(newCameraVector2.x, newCameraVector2.y, transform.position.z);
    }

    private void Update()
    {
        playerVelocity = playerRigidbody2D.velocity;


        if (flipPlayer != playerController.GetIsFacingRight())
        {
            flipPlayer = playerController.GetIsFacingRight();
            temp = 0;
        }

        if (playerController.GetIsFacingRight())
        {
            virtualVector2.x = player.position.x + 2.5f;
        }
        else
        {
            virtualVector2.x = player.position.x - 2.5f;
        }

        if (playerVelocity.y > 2)
        {
            virtualVector2.y = player.position.y + 0.25f;
        }
        else
        {
            virtualVector2.y = player.position.y;
        }
    }

    private void FixedUpdate()
    {
        temp = Mathf.Lerp(temp, 6, 6 * Time.deltaTime);
        newCameraVector2 = Vector2.Lerp(newCameraVector2, virtualVector2, temp * Time.deltaTime);
        transform.position = new Vector3(newCameraVector2.x, newCameraVector2.y,transform.position.z);
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            if (!Application.isPlaying)
            {
                virtualVector2 = player.transform.position;
                virtualVector2.x = player.position.x + 2.5f;
                newCameraVector2 = player.transform.position;
                //transform.position = virtualVector2;
            }

            Gizmos.DrawWireSphere(virtualVector2, 0.2f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(newCameraVector2, 0.2f);
        }
    }
}
