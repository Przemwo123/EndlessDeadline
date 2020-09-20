using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractiveObjects : MonoBehaviour
{
    public GameObject DeathSound;

    protected enum State
    {
        Sleep,
        Dead
    }
    protected State _currentState;

    [Serializable]
    public struct ObjectsStats
    {
        public string nameObject;
        public int hp;
        public int maxHp;
    }

    [Header("Definiowane w panelu")]
    public ObjectsStats objectsStats;

    protected Rigidbody2D _rigidbody2D;

    private int _damageDirection;
    private float _knockbackStartTime;

    protected virtual void Awake()
    {
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    //------ Funkcja odpowiedzialna za obsługę otrzymania obrażeń ------
    protected virtual void TakeDamage(float[] attackDetails)
    {
        if (objectsStats.hp > 0)
        {
            objectsStats.hp -= (int)attackDetails[0];

            if (objectsStats.hp <= 0)
            {
                SwitchState(State.Dead);
                return;
            }

            if (attackDetails[1] == 1)
            {
                if (attackDetails[2] > transform.position.x)
                {
                    _damageDirection = -1;
                }
                else
                {
                    _damageDirection = 1;
                }

                StopCoroutine("UpdateKnockbackState");
                StartCoroutine("UpdateKnockbackState");
            }
        }
    }

    //------ Funkcja odpowiedzialna za Odrzut ------
    IEnumerator UpdateKnockbackState()
    {
        _knockbackStartTime = Time.time;
        _rigidbody2D.velocity = new Vector2(_damageDirection, _rigidbody2D.velocity.y);
        yield return new WaitForSeconds(0.1f);
    }

    //------ Funkcje odpowiedzialne za śmierć ------
    protected virtual void EnterDeadState()
    {
        Instantiate(DeathSound, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    protected virtual void UpdateDeadState()
    {

    }

    protected virtual void ExitDeadState()
    {

    }

    //------ Zmiana aktualnego statusu ------
    protected void SwitchState(State state)
    {
        switch (_currentState)
        {
            case State.Dead:
                ExitDeadState();
                break;
        }

        switch (state)
        {
            case State.Dead:
                EnterDeadState();
                break;
        }

        _currentState = state;
    }
}
