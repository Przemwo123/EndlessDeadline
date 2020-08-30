using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer01 : MonoBehaviour
{
    public Transform player;

    public float maxIdleDistance = 2;
    [Range(10,20)]
    public float followSpeed = 15;
    public float lookCamDstX = 2;

    private Vector3 targetPos;
    private Vector2 currentPosCamera;
    private PlayerController playerController;
    private Rigidbody2D playerRigidbody2D;
    private bool currentLookPlayer;
    private Vector3 currentIdlePos;
    private Vector2 velocity = Vector2.zero;

    float rbPlayerVelocity, rbpv, smoothLookVelocityX = 0;

    private void Start()
    {
        if (player == null)
            player = GameObject.Find("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        playerRigidbody2D = player.GetComponent<Rigidbody2D>();
        targetPos = player.position;
        currentPosCamera = targetPos;
    }

    

    private void LateUpdate()
    {
        if ((player.position - currentIdlePos).sqrMagnitude > (maxIdleDistance * maxIdleDistance))
        {
            targetPos = player.position;

            if (currentLookPlayer != playerController.GetIsFacingRight())
                currentLookPlayer = playerController.GetIsFacingRight();

            if (playerRigidbody2D.velocity.magnitude <= 0)
                currentIdlePos = targetPos;

            if (currentLookPlayer == true)
                smoothLookVelocityX = lookCamDstX;
            else
                smoothLookVelocityX = -lookCamDstX;

            if (playerRigidbody2D.velocity.magnitude > 0)
                rbPlayerVelocity = Mathf.SmoothDamp(rbPlayerVelocity, playerRigidbody2D.velocity.magnitude, ref rbpv, 0.6f);
            else
                rbPlayerVelocity = Mathf.SmoothDamp(rbPlayerVelocity, 0, ref rbPlayerVelocity, 0.6f);
        }
    }

    private void FixedUpdate()
    {
        currentPosCamera = Vector2.SmoothDamp(currentPosCamera, new Vector2(targetPos.x + smoothLookVelocityX,targetPos.y), ref velocity, (followSpeed - (rbPlayerVelocity / 2))*Time.fixedDeltaTime);
        transform.position = new Vector3(currentPosCamera.x, currentPosCamera.y, transform.position.z);
    }

    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            if (!Application.isPlaying)
                currentIdlePos = player.position;

            Gizmos.DrawWireSphere(player.position, 0.1f);

            if (!((player.position - currentIdlePos).sqrMagnitude > (maxIdleDistance * maxIdleDistance)))
                Gizmos.DrawWireSphere(currentIdlePos, maxIdleDistance);

            if (Application.isPlaying)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(currentPosCamera, 0.1f);//Aktualna pozycja kamery

                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(new Vector3(targetPos.x + smoothLookVelocityX, currentPosCamera.y, targetPos.z), 0.1f);//Docelowa pozycja kamery
            }
        }
    }
}