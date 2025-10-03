using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CinemachineZoomFunction : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _cinemachineVirtualCamera = null;

    //[SerializeField]
    //private CinemachineInputProvider _cinemachineInputProvider = null;

    [SerializeField]
    private float _sensitivity = 1f;

    [SerializeField]
    private float _minDistance = 1f;

    [SerializeField]
    private float _maxDistance = 5f;


    public float Sensitivity
    {
        get => _sensitivity;
        set => _sensitivity = value;
    }

    //private void OnEnable()
    //{
    //    // Controllerの受付有効
    //    _cinemachineInputProvider.ZAxis.action.Enable();
    //}

    //private void OnDisable()
    //{
    //    // Controllerの受付無効
    //    _cinemachineInputProvider.ZAxis.action.Disable();
    //}

    private void Awake()
    {
        //var cinemachineComponent = _cinemachineVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        //if (cinemachineComponent is CinemachineFramingTransposer cinemachineFramingTransposer)
        //{
        //	_cinemachineInputProvider.ZAxis.action.performed += context =>
        //	{
        //		float distance = Mathf.Clamp(cinemachineFramingTransposer.m_CameraDistance + context.ReadValue<float>() * _sensitivity, _minDistance, _maxDistance);
        //		cinemachineFramingTransposer.m_CameraDistance = distance;
        //	};
        //}
        //else
        //{
        //	Debug.LogWarning($"{nameof(CinemachineFramingTransposer)} が存在しません。");
        //}

        //if (_cinemachineInputProvider.ZAxis.action != null)
        //{
        //    _cinemachineInputProvider.ZAxis.action.performed += OnZoom;
        //}
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (_cinemachineVirtualCamera == null) return;

        var framingTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (framingTransposer == null) return;

        float delta = context.ReadValue<float>();
        float newDistance = Mathf.Clamp(
            framingTransposer.m_CameraDistance + delta * _sensitivity,
            _minDistance,
            _maxDistance);

        framingTransposer.m_CameraDistance = newDistance;
    }

}
