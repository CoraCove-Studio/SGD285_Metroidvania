using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHits : MonoBehaviour
{
    [SerializeField] Collider StrikingPoint_1;
    [SerializeField] AudioSource swordStrike;
    [SerializeField] AudioSource footStep;

    public void TurnOnStrikePoint_1()
    {
        StrikingPoint_1.enabled = true;
    }

    public void TurnOffStrikePoint_1()
    {
        StrikingPoint_1.enabled = false;
    }

    public void On_Sword_Strike()
    {
        swordStrike.Play();
    }

    public void On_FootStep()
    {
        footStep.Play();
    }
}
