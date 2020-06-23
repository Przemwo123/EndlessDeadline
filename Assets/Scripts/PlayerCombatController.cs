using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    public Transform firePoint;
    public Transform bullet;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, firePoint.position, Quaternion.Euler(bullet.rotation.x, GetComponent<PlayerController>().GetIsFacingRight() ? 0 : 180, bullet.rotation.z));
        }
    }
}
