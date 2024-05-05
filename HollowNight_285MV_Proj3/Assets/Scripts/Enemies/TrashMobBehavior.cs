///////////////////////
// Script Contributors:
// Emma Cole
// Rachel Huggins (Animations)
///////////////////////

using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class TrashMobBehavior : MonoBehaviour
{
    enum AIState { idle, attack};

    private AIState currentState = AIState.idle;
    private NavMeshAgent agent;

    private Animation anim;
    private bool isDead = false;
    
    [SerializeField] private GameObject playerObject;
    private float distanceFromPlayer;
    private bool playerSeen;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animation>();

        anim.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead == false)
        {
            PlayerSense();
            HandleStates(currentState);
        }
    }

    private void PlayerSense()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
        if(distanceFromPlayer <= 6.0f && !playerSeen)
        {
            playerSeen = true;
            currentState = AIState.attack;
            InvokeRepeating(nameof(ShootAtPlayer), 0f, 4.0f);
        }
        else
        {
            return;
        }
    }

    private void HandleStates(AIState state)
    {
        switch (state)
        {
            case AIState.idle:
                break;
            case AIState.attack:
                agent.SetDestination(playerObject.transform.position);
                break;
            default:
                print("unknown state encountered for trash mob behavior");
                break;
        }
    }

    private void ShootAtPlayer()
    {
        Vector3 rayStart = transform.position;
        Vector3 rayDirection = (playerObject.transform.position - transform.position).normalized;
        float rayLength = 10f;

        // Perform the raycast
        if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hitInfo, rayLength))
        {
            // Check if the collider of the hit object has the tag "player"
            if (hitInfo.collider.CompareTag(TagManager.PLAYER))
            {
                PlayAnimationByName("attack");
                print("hit player");
                PlayerManager pm = hitInfo.collider.gameObject.GetComponentInParent<PlayerManager>();
                if (pm != null)
                {
                    pm.TakeDamage(15);
                }
            }
            else
            {
                Debug.Log("Did not hit player, hit: " + hitInfo.collider.name);
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }

    }

    //called when player hits the enemy
    public void Die()
    {
        isDead = true;
        agent.enabled = false;
        CancelInvoke(nameof(ShootAtPlayer));
        PlayAnimationByName("die");
        agent.areaMask = 0;
        Invoke("OnDisableDeath", 5f);
    }

    void PlayAnimationByName(string animationName)
    {
        // Checking if animation exists and will play it
        if (anim[animationName] != null)
        {
            anim.Play(animationName);
        }
        else
        {
            Debug.Log("Animation not found: " + animationName);
        }
    }

    private void OnDisableDeath()
    {
       gameObject.SetActive(false);
    }

    void Run()
    {
        PlayAnimationByName("run");
    }
}
