using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class EnemyDefinition : MonoBehaviour
{
    protected enum State
    {
        Moving,
        Dead,
        InAir
    }
    protected State _currentState;

    [Serializable]
    public struct EnemyStats
    {
        public string nameEnemy;
        public int hp;
        public int maxHp;
        public Vector3 startingPoint;
        public float movementSpeed;
        [Range(0.0f, 100.0f)]
        public float maxWalkingDistance;
        public float viewDistance;
        [Range(0.0f, 360.0f)]
        public float viewAngle;
        [Range(0.0f, 360.0f)]
        public float viewDirection;
    }

    [Header("Definiowane w panelu")]
    public EnemyStats enemyStats;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public Transform feetPos;

    private float
        _groundCheckDistance = 1.0f,
        _wallCheckDistance = 0.1f;

    [SerializeField]
    private Transform _groundCheck = null;
    [SerializeField]
    private Transform _wallCheck = null;

    [SerializeField]
    protected LayerMask _whatIsGround  = 8;

    [Header("Definiowane dynamicznie")]
    private bool _isViewLeft = false;
    protected Transform _visibleTarget;

    protected Rigidbody2D _rigidbody2D;
    private int _directionOfMoving, _damageDirection;
    private bool _groundDetected, _wallDetected, isKnockback;

    private float _knockbackStartTime;

    protected virtual void Awake()
    {
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        if (enemyStats.startingPoint.x == 0 && enemyStats.startingPoint.y == 0 && enemyStats.startingPoint.z == 0)
            enemyStats.startingPoint = transform.position;

        _directionOfMoving = (int)this.transform.localScale.x;

        if (_directionOfMoving == 1) _isViewLeft = false;
        else _isViewLeft = true;
    }

    //------ Wirtualne funkcje Update ------
    protected virtual void EnemyUpdate() { }
    protected virtual void EnemyFixedUpdate() { }

    protected virtual void Update()
    {
        EnemyUpdate();
    }
    protected virtual void FixedUpdate()
    {
        if (_currentState != State.Dead)
        {
            if (!Physics2D.OverlapCircle(feetPos.position, 0.1f, _whatIsGround))
            {
                if (_visibleTarget != null) _visibleTarget = null;
                SwitchState(State.InAir);
                return;
            }
        }
        EnemyFixedUpdate();
    }

    //------ Funkcje odpowiedzialne za poruszanie się ------
    protected virtual void EnterMovingState()
    {

    }

    protected virtual void UpdateMovingState()
    {
        _groundDetected = Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _whatIsGround);
        _wallDetected = Physics2D.Raycast(_wallCheck.position, transform.right, _wallCheckDistance, _whatIsGround);

        if (enemyStats.maxWalkingDistance > 0)
        {
            if (enemyStats.startingPoint.y != transform.position.y)
            {
                enemyStats.startingPoint.y = transform.position.y;
            }

            if (enemyStats.maxWalkingDistance < transform.position.x - enemyStats.startingPoint.x && _isViewLeft == false)
            {
                Flip();
            }
            else if (-enemyStats.maxWalkingDistance > transform.position.x - enemyStats.startingPoint.x && _isViewLeft == true)
            {
                Flip();
            }
        }

        if (!_groundDetected || _wallDetected)
        {
            if ((this.transform.position - enemyStats.startingPoint).sqrMagnitude > (enemyStats.maxWalkingDistance * enemyStats.maxWalkingDistance))
                enemyStats.startingPoint = transform.position;
            Flip();
        }

        if(!isKnockback) _rigidbody2D.velocity = new Vector2(enemyStats.movementSpeed * (_isViewLeft ? -1:1), _rigidbody2D.velocity.y);
    }

    protected virtual void ExitMovingState()
    {

    }

    //------ Funkcje odpowiedzialne za śmierć ------
    protected virtual void EnterDeadState()
    {
        if (_visibleTarget != null) _visibleTarget = null;
        Destroy(gameObject);
    }

    protected virtual void UpdateDeadState()
    {

    }

    protected virtual void ExitDeadState()
    {

    }

    //------ INNE FUNKCJE ------------------------------------------------------------

    //------ Funkcja odpowiedzialna za obsługę otrzymania obrażeń ------
    protected virtual void TakeDamage(float[] attackDetails)
    {
        if (enemyStats.hp > 0)
        {
            enemyStats.hp -= (int)attackDetails[0];

            if (enemyStats.hp <= 0)
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
        isKnockback = true;
        _knockbackStartTime = Time.time;
        _rigidbody2D.velocity = new Vector2(enemyStats.movementSpeed*_damageDirection, _rigidbody2D.velocity.y);
        yield return new WaitForSeconds(0.1f);
        isKnockback = false;
    }

    //------ Funkcje odpowiedzialne za spadanie ------

    protected virtual void EnterInAir()
    {

    }

    protected virtual void UpdateInAir()
    {
        if (Physics2D.OverlapCircle(feetPos.position, 0.1f, _whatIsGround))
        {
            SwitchState(State.Moving);
            return;
        }
    }

    protected virtual void ExitInAir()
    {

    }

    //------ Funkcja odpowiedzialna za obrót w przeciwny kierunek ------
    protected virtual void Flip()
    {
        
        _directionOfMoving *= -1;
        _isViewLeft = !_isViewLeft;
        Vector3 _setFlip = new Vector3(_isViewLeft ? -1:1, 1, 1);
        this.transform.localScale = _setFlip;
    }

    //------ Funkcja odpowiedzialna za wykrywanie gracza ------

    protected void FindVisibleTargets()
    {
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, enemyStats.viewDistance, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            Vector3 dirToTarget = (target.position - transform.position);

            float angle = Vector3.Angle(Quaternion.Euler(0, 0, _isViewLeft ? -enemyStats.viewDirection : enemyStats.viewDirection) * (_isViewLeft ? Vector2.left : Vector2.right), dirToTarget);

            if (angle <= enemyStats.viewAngle * 0.5f)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask) && _visibleTarget == null)
                {
                    _visibleTarget = target;
                }else if (Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    _visibleTarget = null;
                    return;
                }
            }
            else if(_visibleTarget != null)
            {
                _visibleTarget = null;
                return;
            }
        }
    }

    //------ Zmiana aktualnego statusu ------
    protected void SwitchState(State state)
    {
        switch (_currentState)
        {
            case State.Moving:
                ExitMovingState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
            case State.InAir:
                ExitInAir();
                break;
        }

        switch (state)
        {
            case State.Moving:
                EnterMovingState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
            case State.InAir:
                EnterInAir();
                break;
        }

        _currentState = state;
    }


    //------ Funkcja UnityEditor odpowiedzialna za wyświetlanie linii pomocniczych ------
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(_groundCheck.position, new Vector2(_groundCheck.position.x, _groundCheck.position.y - _groundCheckDistance));
        Gizmos.DrawLine(_wallCheck.position, new Vector2(_wallCheck.position.x + _wallCheckDistance, _wallCheck.position.y));

        /*Handles.color = new Color(0, 0, 0, 0.05f);
        if(Application.isPlaying) Handles.DrawSolidDisc(enemyStats.startingPoint, Vector3.back, enemyStats.maxWalkingDistance);
        else Handles.DrawSolidDisc(this.transform.position, Vector3.back, enemyStats.maxWalkingDistance);*/
        
        //kąt i zasięg widzenia
        if (!Application.isPlaying)
        {
            _directionOfMoving = (int)this.transform.localScale.x;
            if (_directionOfMoving == 1) _isViewLeft = false;
            else _isViewLeft = true;
        }
        Vector3 forward = _isViewLeft ? Vector2.left : Vector2.right;
        forward = Quaternion.Euler(0, 0, _isViewLeft ? -enemyStats.viewDirection : enemyStats.viewDirection) * forward;

        Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, enemyStats.viewAngle * 0.5f) * forward);

        /*Handles.color = new Color(0, 1.0f, 0, 0.2f);
        Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, enemyStats.viewAngle, enemyStats.viewDistance);*/

        Gizmos.color = Color.red;
        if (_visibleTarget != null)
            Gizmos.DrawLine(transform.position, _visibleTarget.position);
    }
}