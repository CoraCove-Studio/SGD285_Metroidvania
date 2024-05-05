using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSwordCollision : MonoBehaviour
{
    public GameObject hit_FX;
    private bool hit_FX_Created = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.BOSS))
        {
            BossController bossController = other.gameObject.GetComponentInParent<BossController>();
            bossController.TakeDamage(15);
        }
        else if (other.gameObject.CompareTag(TagManager.TRASHMOB))
        {
            TrashMobBehavior mob = other.GetComponent<TrashMobBehavior>();
            mob.Die();
        }
    }

    //void CreateSwordTrail(Vector3 whereHitOccured)
    //{
    //    if (!hit_FX_Created)
    //    {
    //        hit_FX_Created = true;
    //        Instantiate(hit_FX, whereHitOccured, Quaternion.identity);
    //        Invoke("ResetHitEffect", 1.5f);  // Rename method for clarity
    //    }
    //}

    //void ResetHitEffect()
    //{
    //    // Reset only the effect creation state, do not deactivate the gameObject
    //    hit_FX_Created = false;
    //}
}
