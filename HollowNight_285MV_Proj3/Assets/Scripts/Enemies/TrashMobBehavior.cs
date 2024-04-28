///////////////////////
// Script Contributors:
// Emma Cole
///////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrashMobBehavior : MonoBehaviour
{
    enum AIState { idle, attack};

    private AIState currentState = AIState.idle;
    private NavMeshAgent agent;
    
    [SerializeField] private GameObject playerObject;
    private float distanceFromPlayer;
    private bool playerSeen;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerSense();
        HandleStates(currentState);
    }

    private void PlayerSense()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
        if(distanceFromPlayer <= 10.0f && !playerSeen)
        {
            playerSeen = true;
            currentState = AIState.attack;
            InvokeRepeating("ShootAtPlayer", 0f, 4.0f);
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
                print("hit player");
                //Player's Take Damage Here
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
        CancelInvoke("ShootAtPlayer");
        this.gameObject.SetActive(false);
    }
}
