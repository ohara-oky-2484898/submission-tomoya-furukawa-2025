using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleController : MonoBehaviour
{
    private GrappleModel model;
    private GrappleView view;

    [Header("Input Actions")]
    public InputActionProperty grappleAction;

    void Awake()
    {
        model = GetComponent<GrappleModel>();
        view = GetComponent<GrappleView>();
    }

    private void OnEnable()
    {
        grappleAction.action.Enable();
    }

    private void OnDisable()
    {
        grappleAction.action.Disable();
    }

    void Update()
    {
        if (grappleAction.action.WasPressedThisFrame())
        {
            model.StartGrapple();
        }
        else if (grappleAction.action.WasReleasedThisFrame())
        {
            model.StopGrapple();
        }
    }
}
