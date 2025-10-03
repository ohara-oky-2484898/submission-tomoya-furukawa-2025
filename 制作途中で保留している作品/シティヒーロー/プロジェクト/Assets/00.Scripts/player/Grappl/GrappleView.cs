using UnityEngine;

public class GrappleView : MonoBehaviour
{
    private LineRenderer lr;
    private GrappleModel grappleModel;
    private Vector3 currentGrapplePosition;

    // LineRendererに関する設定
    public int quality = 10;
    public float waveHeight = 0.5f;
    public float waveCount = 3f;
    public AnimationCurve affectCurve;

    private bool ropeFullyExtended = false;
    private float ropeExtendSpeed = 12f;
    private float ropeTimer = 0f;
    private float ropeDuration = 0.5f; // しなりからピンと張るまでの時間


    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        grappleModel = GetComponent<GrappleModel>();
    }

    void LateUpdate()
    {
        if (grappleModel.IsGrappling())
        {
            if (ropeTimer == 0f)
            {
                currentGrapplePosition = grappleModel.gunTipTrans.position; // 初期化
            }
            ropeTimer += Time.deltaTime;
            if (ropeTimer >= ropeDuration)
            {
                ropeFullyExtended = true;
            }
        }
        else
        {
            ropeTimer = 0f;
            ropeFullyExtended = false;
        }

        DrawRope();
    }


    void DrawRope()
    {
        // グラップルが発生していない場合は描画を止める
        if (!grappleModel.IsGrappling())
        {
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }

        // 描画開始点（gunTip）から描画を開始
        Vector3 gunTipPosition = grappleModel.gunTipTrans.position;
        Vector3 grapplePoint = grappleModel.GetGrapplePoint();


        //currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 12f);
        // Ropeの "進行中の位置"
        currentGrapplePosition = Vector3.Lerp(
            currentGrapplePosition,
            grapplePoint,
            Time.deltaTime * ropeExtendSpeed
        );

		if (lr.positionCount == 0)
		{
			lr.positionCount = quality + 1;
		}

		//var up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

		//for (var i = 0; i < quality + 1; i++)
		//{
		//    var delta = i / (float)quality;
		//    var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * 0.5f * affectCurve.Evaluate(delta);
		//    lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
		//}
		Vector3 direction = grapplePoint - gunTipPosition;
        Vector3 up = Quaternion.LookRotation(direction.normalized) * Vector3.up;

        for (int i = 0; i <= quality; i++)
        {
            float delta = i / (float)quality;

            float waveFactor = ropeFullyExtended ? 0f : 1f; // 時間が来たら波を消す
            float wave = Mathf.Sin(delta * waveCount * Mathf.PI) * waveHeight * waveFactor * affectCurve.Evaluate(delta);

            Vector3 offset = up * wave;
            Vector3 point = Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset;
            lr.SetPosition(i, point);
        }
    }
}
