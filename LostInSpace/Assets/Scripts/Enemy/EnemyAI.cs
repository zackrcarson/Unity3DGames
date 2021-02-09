﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    // Config Parameters
    [SerializeField] float chaseRange = 10f;
    [SerializeField] float turnSpeed = 1f;
    [SerializeField] float waitTimeToReturnToStart = 3f;

    // Cached references
    int numAttackAnimations = 3;
    EnemyHealth enemyHealth = null;
    NavMeshAgent navMeshAgent = null;
    Transform target = null;
    Animator animator = null;
    Vector3 startingPosition = new Vector3(0f, 0f, 0f);

    // State Variables
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    bool playerDead = false;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;

        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponentInChildren<Animator>();

        target = FindObjectOfType<PlayerHealth>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDead) { return; }
        if (enemyHealth.IsDead())
        {
            enabled = false;
            navMeshAgent.enabled = false;
            return;
        }

        distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
    }

    private void EngageTarget()
    {
        FaceTarget();

        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }
        else
        {
            AttackTarget();
        }
    }

    private void FaceTarget()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0f, directionToTarget.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void ChaseTarget()
    {
        navMeshAgent.SetDestination(target.position);

        animator.SetInteger("attack", 0);
        animator.SetTrigger("move");
    }

    private void AttackTarget()
    {
        animator.SetInteger("attack", GetRandomAttackAnimation());
    }

    public IEnumerator PlayerDead()
    {
        playerDead = true;
        
        animator.SetTrigger("howl");

        yield return new WaitForSeconds(waitTimeToReturnToStart);

        navMeshAgent.speed /= 2; 
        animator.SetTrigger("backHome");

        navMeshAgent.SetDestination(startingPosition);

        StartCoroutine(IdleWhenHome());
    }

    private IEnumerator IdleWhenHome()
    {
        yield return new WaitUntil(() => navMeshAgent.velocity.sqrMagnitude <= Mathf.Epsilon);

        animator.SetTrigger("isHome");
    }

    // Called via string reference BroadCastMessage
    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    private int GetRandomAttackAnimation()
    {
        return Random.Range(1, numAttackAnimations + 1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, GetComponent<NavMeshAgent>().stoppingDistance);
    }
}
