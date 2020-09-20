using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100;
    public float damage = 1;
    public Transform[] effect;
    public GameObject HitEnemySound;

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (onScreen == false)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
            case "ObjectDestroy":
                DealDamage(collision);
                break;
            default:
                Instantiate(effect[1], transform.position, transform.rotation);
                break;
        }

        Destroy(gameObject);
    }

    private void DealDamage(Collider2D collision)
    {
        Instantiate(HitEnemySound, transform.position, transform.rotation);
        Instantiate(effect[0], transform.position, transform.rotation);
        float[] temp = new float[3];
        temp[0] = damage;//obrażenia
        temp[1] = 1;//Czy ma być odrzut 1-tak, 0-nie
        temp[2] = GameObject.Find("Player").transform.position.x;//Pozycja gracza
        collision.SendMessage("TakeDamage", temp);
    }
}