using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    public InputAction zoomAction;
    public float zoomSpeed = 10f;

    private void OnEnable()
    {
        zoomAction.Enable();
    }

    private void OnDisable()
    {
        zoomAction.Disable();
    }

    private void Update()
    {
        float zoomValue = zoomAction.ReadValue<float>();

        Camera.main.fieldOfView -= zoomValue * zoomSpeed * Time.deltaTime;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 30f, 90f);
    }
}
