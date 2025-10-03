using UnityEngine;
using UnityEngine.InputSystem;

public class MeasuringTapeController : MonoBehaviour
{
    public MeasuringTapeView tapeView;

    private bool isExtending = true;
    private bool isButtonHeld = false;

    void Update()
    {
        if (isButtonHeld)
        {
            float delta = (isExtending ? 1 : -1) * tapeView.Speed * Time.deltaTime;
            tapeView.ApplyLengthChange(delta);
        }
    }

    public void OnExtendToggle(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isButtonHeld = true;
        }
        else if (context.canceled)
        {
            isButtonHeld = false;
            isExtending = !isExtending;
        }
    }
}
