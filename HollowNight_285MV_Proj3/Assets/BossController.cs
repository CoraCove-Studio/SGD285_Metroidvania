///////////////////////
// Script Contributors:
// 
// Zeb Baukhagen
// 
///////////////////////

using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float movementSpeed = 1.5f;
    [SerializeField] private Collider clawColliderLeft;
    [SerializeField] private Collider clawColliderRight;
    [SerializeField] private Collider mouthCollider;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float detectionRadius = 30f;  // Distance to consider the player in the arena
    private Animator animator;
    private Transform playerTransform;
    private int currentHealth;
    private bool isPlayerInArena = false;
    private readonly float closeDistance = 8f;
    private readonly float rotationSpeed = 5f;  // Adjust this value to get the desired rotation speed
    private readonly float attackCooldown = 5.0f; // Seconds between attacks
    private float lastAttackTime = 0f;
    private Coroutine stateMachineCoroutine;
    private bool movingTowardsPlayer = false;
    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        clawColliderLeft.enabled = false;
        clawColliderRight.enabled = false;
        mouthCollider.enabled = false;
        StartCoroutine(CheckPlayerDistance());
    }

    private void FixedUpdate()
    {
        if (movingTowardsPlayer && isDead == false)
        {
            MoveTowardsPlayer();
        }
    }

    void OnEnable()
    {
        stateMachineCoroutine = StartCoroutine(StateMachine());
    }

    void OnDisable()
    {
        if (stateMachineCoroutine != null)
        {
            StopCoroutine(stateMachineCoroutine);
        }
    }

    private IEnumerator CheckPlayerDistance()
    {
        while (isDead == false)
        {
            float distance = Vector3.Distance(playerTransform.position, transform.position);
            bool shouldReact = isPlayerInArena;
            isPlayerInArena = distance <= detectionRadius;

            if (!shouldReact && isPlayerInArena)
            {
                animator.ResetTrigger("Idle");
                stateMachineCoroutine ??= StartCoroutine(StateMachine());
            }
            else if (shouldReact && !isPlayerInArena)
            {
                animator.SetTrigger("Idle");
                animator.SetFloat("Movement", 0f);
                if (stateMachineCoroutine != null)
                {
                    StopCoroutine(stateMachineCoroutine);
                    stateMachineCoroutine = null;
                }
            }

            yield return new WaitForSeconds(0.5f);  // Check every half-second
        }
    }

    private IEnumerator StateMachine()
    {
        while (isPlayerInArena && isDead == false)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            float timeSinceLastAttack = Time.time - lastAttackTime;

            if (currentHealth <= 0)
            {
                Die();
                yield break; // Ensure the coroutine stops after death
            }

            if (timeSinceLastAttack >= attackCooldown)
            {
                if (distanceToPlayer < closeDistance)
                {
                    // Randomly choose between Basic Attack and Claw Attack
                    if (Random.value > 0.5f)
                    {
                        BasicAttack();
                    }
                    else
                    {
                        ClawAttack();
                    }
                    lastAttackTime = Time.time;
                }
            }

            // Move towards player if not within attack range or waiting on cooldown
            if (distanceToPlayer >= closeDistance || Time.time < lastAttackTime + attackCooldown)
            {
                movingTowardsPlayer = true;
            }
            else
            {
                movingTowardsPlayer = false;
                if (Random.value > 0.5f)
                {
                    Scream();
                }
                else
                {
                    GoIdle();
                }
            }

            yield return new WaitForSeconds(0.5f);  // Check every half-second
        }
    }

    private void MoveTowardsPlayer()
    {
        animator.SetFloat("Movement", 1.0f);
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        // Rotate towards the player smoothly
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Only move if far enough away to require movement, preventing small jittery movements
        if (Vector3.Distance(transform.position, playerTransform.position) > 1.0f)
        {
            rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
        }
    }

    private void Scream()
    {
        animator.SetTrigger("Scream");
    }

    private void GoIdle()
    {
        animator.SetTrigger("Idle");
        animator.SetFloat("Movement", 0f);
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        isPlayerInArena = false;
        isDead = true;
    }

    private void BasicAttack()
    {
        animator.SetTrigger("BasicAttack");
        mouthCollider.enabled = true;
        Invoke(nameof(DisableColliders), 2.0f);
    }

    private void ClawAttack()
    {
        animator.SetTrigger("ClawAttack");
        clawColliderLeft.enabled = true;
        clawColliderRight.enabled = true;
        Invoke(nameof(DisableColliders), 2.0f);
    }

    private void DisableColliders()
    {
        clawColliderLeft.enabled = false;
        clawColliderRight.enabled = false;
        mouthCollider.enabled = false;
    }

    public void TakeDamage(int damage)
    {
        print("Boss took damage!");
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        animator.SetTrigger("GetHit");
        if (currentHealth <= 0)
            Die();
        print("Bosses current health: " + currentHealth);
    }
}