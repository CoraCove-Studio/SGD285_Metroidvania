using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float xRotation = 0f;
    [SerializeField] private float xSensitivity = 25f;
    [SerializeField] private float ySensitivity = 25f;
    [SerializeField] private float degreeClamp = 52f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        // calculating the camera rotation for looking up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -degreeClamp, degreeClamp);

        // this applies to the cameras transform
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // rotating the camera to look left and right
        transform.Rotate((mouseX * Time.deltaTime) * xSensitivity * Vector3.up);
    }
}
