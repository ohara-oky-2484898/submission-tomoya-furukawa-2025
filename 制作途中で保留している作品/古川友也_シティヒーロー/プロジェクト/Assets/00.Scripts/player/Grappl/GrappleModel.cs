using UnityEngine;

public class GrappleModel : MonoBehaviour
{
    private Vector3 grapplePoint;
    private SpringJoint joint;
    public LayerMask whatIsGrappleable;
    public Transform gunTipTrans, cameraTrans, playerTrans;
    private float maxDistance = 100f;

    public void StartGrapple()
    {
        RaycastHit hit;
        // レイキャストをカメラの位置から、カメラの向きに沿って発射
        if (Physics.Raycast(cameraTrans.position, cameraTrans.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = playerTrans.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(gunTipTrans.position, grapplePoint);
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;
        }
    }

    public void StopGrapple()
    {
        if (joint != null)
        {
            Destroy(joint);
        }
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

