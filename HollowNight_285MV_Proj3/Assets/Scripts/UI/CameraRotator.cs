///////////////////////////////////////////
/////// Script Contributors:
/////// Rachel Huggins
///////
///////
///////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
    }
}
