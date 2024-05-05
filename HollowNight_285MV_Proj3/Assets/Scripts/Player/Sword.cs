///////////////////////
// Script Contributors:
// 
// Zeb Baukhagen
// Rachel Huggins
///////////////////////

using UnityEngine;

public class Sword : MonoBehaviour
{
    public LayerMask collisionLayer;

    //public bool is_Player, is_Enemy;

    public GameObject hit_FX;
    private bool hit_FX_Created = false;

    private void Update()
    {
        DetectCollion();
    }

    void DetectCollion()
    {
        //creating an invisible sphere to detect a collision with a gameobject that are set on that layer
        Collider[] hit = Physics.OverlapSphere(transform.position, 5f, collisionLayer);

        if (hit.Length > 0)
        {
            Vector3 hitFX_Pos = hit[0].transform.position;

            Debug.Log("enemy hit");

            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].CompareTag(TagManager.BOSS))
                {
                    CreateSwordTrail(hitFX_Pos);
                    BossController bossController = hit[i].GetComponent<BossController>();
                    bossController.TakeDamage(15);
                }
                else if (hit[i].CompareTag(TagManager.TRASHMOB))
                {
                    CreateSwordTrail(hitFX_Pos);
                    TrashMobBehavior mob = hit[i].GetComponent<TrashMobBehavior>();
                    mob.Die();
                }
            }
        }
    }

    void CreateSwordTrail(Vector3 whereHitOccured)
    {
        if (!hit_FX_Created)
        {
            hit_FX_Created = true;
            Instantiate(hit_FX, whereHitOccured, Quaternion.identity);
            Invoke("ResetHitEffect", 1.5f);  // Rename method for clarity
        }
    }

    void ResetHitEffect()
    {
        // Reset only the effect creation state, do not deactivate the gameObject
        hit_FX_Created = false;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (gameObject.CompareTag(TagManager.BOSS))
    //    {
    //        BossController bossController = other.gameObject.GetComponent<BossController>();
    //        bossController.TakeDamage(15);
    //    }
    //    else if (gameObject.CompareTag(TagManager.TRASHMOB))
    //    {
    //        TrashMobBehavior mob = other.GetComponent<TrashMobBehavior>();
    //        mob.Die();
    //    }
    //}
}
