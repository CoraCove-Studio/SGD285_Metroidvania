using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if (other.CompareTag(TagManager.BOSS))
        {
            BossController bossController = other.gameObject.GetComponent<BossController>();
            bossController.TakeDamage(15);
        }
        else if (other.CompareTag(TagManager.TRASHMOB))
        {
            TrashMobBehavior mob = other.GetComponent<TrashMobBehavior>();
            mob.Die();
        }
    }
}
