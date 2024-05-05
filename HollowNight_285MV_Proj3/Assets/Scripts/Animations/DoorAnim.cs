///////////////////////
// Script Contributors:
// 
// Zeb Baukhagen
// 
///////////////////////

using System.Collections;
using UnityEngine;

public class DoorAnim : MonoBehaviour
{
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private int leftDoorOpenRotation = 90;
    [SerializeField] private int rightDoorOpenRotation = -90;
    [SerializeField] private float duration = 2.0f; // Duration of the animation in seconds

    [SerializeField] private bool openDoor = false;

    private void Update()
    {
        if (openDoor)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        StartCoroutine(AnimateOpening());
    }

    private IEnumerator AnimateOpening()
    {
        float timeElapsed = 0;
        Quaternion leftStartRotation = leftDoor.transform.rotation;
        Quaternion rightStartRotation = rightDoor.transform.rotation;
        Quaternion leftEndRotation = Quaternion.Euler(0, leftDoorOpenRotation, 0);
        Quaternion rightEndRotation = Quaternion.Euler(0, rightDoorOpenRotation, 0);

        while (timeElapsed < duration)
        {
            // Update the rotation of each door
            leftDoor.transform.rotation = Quaternion.Lerp(leftStartRotation, leftEndRotation, timeElapsed / duration);
            rightDoor.transform.rotation = Quaternion.Lerp(rightStartRotation, rightEndRotation, timeElapsed / duration);

            timeElapsed += Time.deltaTime;
            yield return null; // Wait for a frame
        }

        // Ensure the doors are exactly at the target rotation after finishing
        leftDoor.transform.rotation = leftEndRotation;
        rightDoor.transform.rotation = rightEndRotation;
    }
}
