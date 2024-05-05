using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TriggerHits : MonoBehaviour
{
    private bool hit_FX_Created = false;

    [SerializeField] Transform hitFX_Pos;
    [SerializeField] Collider StrikingPoint_1;
    [SerializeField] AudioSource swordStrike;
    [SerializeField] AudioSource footStep;
    [SerializeField] GameObject hit_FX;

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

    public void CreateSwordTrail()
    {   
        hit_FX_Created = true;
        Instantiate(hit_FX, hitFX_Pos.position, Quaternion.identity);
        Invoke("ResetHitEffect", 1.5f);  // Rename method for clarity
}
    void ResetHitEffect()
    {
        // Reset only the effect creation state, do not deactivate the gameObject
        hit_FX_Created = false;
    }
}
