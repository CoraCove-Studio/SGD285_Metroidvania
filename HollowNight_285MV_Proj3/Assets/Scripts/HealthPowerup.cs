using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER))
        {
            PlayerManager pm = other.gameObject.GetComponent<PlayerManager>();
            pm.Heal(20);
            gameObject.SetActive(false);
        }
    }
}
