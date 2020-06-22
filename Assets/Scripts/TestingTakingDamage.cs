using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingTakingDamage : MonoBehaviour
{
    public GameObject enemy;
    public int damage;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            float[] temp = new float[3];
            temp[0] = damage;//obrażenia
            temp[1] = 1;//Czy ma być odrzut 1-tak, 0-nie
            temp[2] = GameObject.Find("Player").transform.position.x;//Pozycja gracza
            enemy.SendMessage("TakeDamage", temp);
        }      
    }
}