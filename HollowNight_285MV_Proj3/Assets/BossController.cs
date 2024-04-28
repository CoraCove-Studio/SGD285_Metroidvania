using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private Collider clawColliderLeft;
    [SerializeField] private Collider clawColliderRight;
    [SerializeField] private Collider mouthCollider;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float detectionRadius = 30f;  // Distance to consider the player in the arena
    private Animator animator;
    private Transform playerTransform;
    private int currentHealth;
    private bool isPlayerInArena = false;
    private readonly float closeDistance = 5f;
    private readonly float farDistance = 8f;
    private readonly float rotationSpeed = 5f;  // Adjust this value to get the desired rotation speed
    private readonly float attackCooldown = 5.0f; // Seconds between attacks
    private float lastAttackTime = 0f;

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

    void Update()
    {
        if (!isPlayerInArena) return;

        StartCoroutine(StateMachine());
    }

    private IEnumerator CheckPlayerDistance()
    {
        while (true)
        {
            float distance = Vector3.Distance(playerTransform.position, transform.position);
            if (distance <= detectionRadius)
            {
                if (!isPlayerInArena)
                {
                    isPlayerInArena = true;
                }
            }
            else
            {
                if (isPlayerInArena)
                {
                    isPlayerInArena = false;
                    animator.SetTrigger("Idle");
                    animator.SetFloat("Movement", 0f);
                }
            }
            yield return new WaitForSeconds(0.5f);  // Check every half-second
        }
    }

    private IEnumerator StateMachine()
    {
        while (isPlayerInArena)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            float timeSinceLastAttack = Time.time - lastAttackTime;

            if (currentHealth <= 0)
            {
                Die();
            }
            else if (timeSinceLastAttack >= attackCooldown)
            {
                if (distanceToPlayer < closeDistance)
                {
                    BasicAttack();
                    lastAttackTime = Time.time; // Reset the last attack time
                }
                else if (distanceToPlayer < farDistance)
                {
                    ClawAttack();
                    lastAttackTime = Time.time; // Reset the last attack time
                }
                else
                {
                    MoveTowardsPlayer();
                }
            }
            else
            {
                // Choose to either scream or go idle if the attack cooldown isn't up
                if (Random.value > 0.5f)
                {
                    Scream();
                }
                else
                {
                    GoIdle();
                }
            }

            yield return null;
        }
    }

    private void MoveTowardsPlayer()
    {
        animator.SetFloat("Movement", 1.0f);
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        // Rotate towards the player smoothly
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Move towards the player
        rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
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
        isPlayerInArena = false;  // Stops further actions
    }

    private void BasicAttack()
    {
        animator.SetTrigger("BasicAttack");
        mouthCollider.enabled = true;
        Invoke(nameof(DisableColliders), 1.0f); // Assume attack duration is 1 second
    }

    private void ClawAttack()
    {
        animator.SetTrigger("ClawAttack");
        clawColliderLeft.enabled = true;
        clawColliderRight.enabled = true;
        Invoke(nameof(DisableColliders), 1.5f); // Assume attack duration is 1.5 seconds
    }

    private void DisableColliders()
    {
        clawColliderLeft.enabled = false;
        clawColliderRight.enabled = false;
        mouthCollider.enabled = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("GetHit");
        if (currentHealth <= 0)
            Die();
    }
}