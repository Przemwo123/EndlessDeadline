using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterfere : MonoBehaviour
{
    [SerializeField]
    private Transform ForwardCheck = null;

    [SerializeField]
    private float ForwardCheckDistance = 0.5f;

    public delegate void newDoorEnter();
    public static event newDoorEnter DoorEnter, DoorExit;
    private bool action = false;

    void Start()
    {

    }

    void Update()
    {
        Vector2 endPos = ForwardCheck.position + Vector3.right * (transform.localScale.x == 1 ? ForwardCheckDistance : -ForwardCheckDistance);
        RaycastHit2D hit = Physics2D.Linecast(ForwardCheck.position, endPos, 1 << LayerMask.NameToLayer("Action"));

        if (hit.collider != null)
        {
            if (hit.collider.tag.Equals("Door") && DoorEnter != null && !action)
            {
                DoorEnter();
                action = true;
            }
        }else if (action)
        {
            DoorExit();
            action = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 endPos = ForwardCheck.position + Vector3.right * (transform.localScale.x == 1 ? ForwardCheckDistance : -ForwardCheckDistance);
        Gizmos.DrawLine(ForwardCheck.position, endPos);
    }
}
