using UnityEngine;
using UnityEngine.InputSystem;

// this turns said player in fixed jumps instead of a smooth spin.
// Snapping is kinder on the stomach in VR than smooth turning.
// Keyboard only for now, so it's really just for testing in the Editor.
public class SnapTurn : MonoBehaviour
{
    public float snapAngle = 45f;  // degrees per turn

    void Update()
    {
        if (Keyboard.current == null) return;  // as there no keyboard on the headset, so skip

        // left arrow or Q turns left
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.qKey.wasPressedThisFrame)
            transform.Rotate(0, -snapAngle, 0);
            

        // right arrow or E turns right
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame)
            transform.Rotate(0, snapAngle, 0);
    }
}