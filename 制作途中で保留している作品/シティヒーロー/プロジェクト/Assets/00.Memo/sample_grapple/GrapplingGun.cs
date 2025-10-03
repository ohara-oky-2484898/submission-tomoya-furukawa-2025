using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingGun : MonoBehaviour
{
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTipTrans, cameraTrans, playerTrans;
    private float maxDistance = 100f;
    private SpringJoint joint;

    [Header("Input Actions")]
    public InputActionProperty grappleAction; // InputActionÇÃéQè∆

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
            StartGrapple();
        }
        else if (grappleAction.action.WasReleasedThisFrame())
        {
            StopGrapple();
        }
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTrans.position, cameraTrans.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = playerTrans.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(playerTrans.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;
        }
    }

    void StopGrapple()
    {
        Destroy(joint);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
