
///////////////////////
// Script Contributors:
// 
// Zeb Baukhagen
// 
///////////////////////

using UnityEngine;

public class BossDamageZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER))
        {
            PlayerManager playerManager = other.GetComponentInParent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.TakeDamage(15);
            }
        }
    }
}
