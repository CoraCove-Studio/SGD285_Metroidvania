using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHits : MonoBehaviour
{
    [SerializeField] GameObject StrikingPoint_1;
    [SerializeField] GameObject StrikingPoint_2;

    public void TurnOnStrikePoint_1()
    {
        StrikingPoint_1.SetActive(true);
    }

    public void TurnOffStrikePoint_1()
    {
        StrikingPoint_1.SetActive(false);
    }
}
