using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public NavMeshAgent agent;
    private Animator enemyAnimator;
    private const float attackDistance = 5f;
    private bool attack = true;
    private int randomAttack = 0;
    private bool stopAttacking = false;
    private int health = 100;
    public EnemyHealthBar healthBar;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;
    bool Victory = false;
    bool Dead = false;  

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        healthBar.SetMaxHealth(health);
        rightHand.SetActive(false);
        leftHand.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!Victory && !Dead)
        {
            transform.LookAt(player.transform.position);
            float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceFromPlayer > attackDistance)
                Chasing();
            else if (distanceFromPlayer < attackDistance && attack && !stopAttacking)
                Attacking();
        }

    }

    private void Chasing()
    {
        agent.SetDestination(player.transform.position);
        enemyAnimator.SetBool("Walking", true);
    }

    private void Attacking()
    {   
        enemyAnimator.SetBool("Walking", false);
        if (attack)
        {
            StartCoroutine(HandsCoroutine());
            enemyAnimator.SetInteger("AttacksDone", randomAttack);
            StartCoroutine(StopMoving());
            enemyAnimator.SetTrigger("Attack");

            attack = false;
        }
        if (!attack)
        {
            StartCoroutine(CooldownCoroutine());
        }
    }

    private IEnumerator StopMoving()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(3f);
        agent.isStopped = false;
    }
    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(2f);
        randomAttack = Random.Range(0, 5);
        attack = true;
    }
    private IEnumerator HandsCoroutine()
    {
        rightHand.SetActive(true);
        leftHand.SetActive(true);
        yield return new WaitForSeconds(1f);
        rightHand.SetActive(false);
        leftHand.SetActive(false);  
    }

    public void PlayerIsDead()
    {
        Victory = true;
        enemyAnimator.SetBool("Victory", Victory);
        stopAttacking = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SendMessage("PlayerGotHit");
        }
    }

    private void EnemyGotHit()
    {
        health -= 5;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            Dead = true;
            stopAttacking = true;
            enemyAnimator.SetBool("Dead", Dead);
            player.SendMessage("EnemyIsDead");
        }
    }
}
