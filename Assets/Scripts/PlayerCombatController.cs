using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    public Transform firePoint;
    public Transform bullet;

    public float damageAttackMelee;
    public float delayAttackMelee;

    public Vector2 attackRange;
    public Transform attackPoint;

    public LayerMask _enemyLayer;

    private bool isAttacking;

    public Transform[] effect;

    public GameObject meleeSound;
    public GameObject HitEnemySound;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackMelee());
        }
        if (Input.GetKeyDown(KeyCode.E) && !isAttacking)
        {
            Instantiate(bullet, firePoint.position, Quaternion.Euler(bullet.rotation.x, GetComponent<PlayerController>().GetIsFacingRight() ? 0 : 180, bullet.rotation.z));
        }
    }

    //------ Light Attack --------
    IEnumerator AttackMelee()
    {
        animator.SetBool("IsSwordAttack", isAttacking);

        

        yield return new WaitForSeconds(delayAttackMelee/2);

        Instantiate(meleeSound, transform.position, transform.rotation);

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, 0, _enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Instantiate(HitEnemySound, transform.position, transform.rotation);
            Instantiate(effect[0], enemy.transform.position, enemy.transform.rotation);
            float[] temp = new float[3];
            temp[0] = damageAttackMelee;//obrażenia
            temp[1] = 1;//Czy ma być odrzut 1-tak, 0-nie
            temp[2] = GameObject.Find("Player").transform.position.x;//Pozycja gracza
            enemy.SendMessage("TakeDamage", temp);
        }

        yield return new WaitForSeconds(delayAttackMelee);

        //------------------------------------------------//
        isAttacking = false;
        animator.SetBool("IsSwordAttack", isAttacking);
    }


    //------Rysowanie zasiegu ataku------
    private void OnDrawGizmos()//Selected
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireCube(attackPoint.position, attackRange);
    }
}
