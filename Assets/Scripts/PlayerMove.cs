using UnityEngine;
using UnityEngine.InputSystem;

// WASD walking~ only really for testing in the Editor since there's no keyboard on said headset.
// Uses the new Input System as the old Input.GetKey version crashed on the Quest.
public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        if (Keyboard.current == null) return;  // no keyboard (e.g. on the Quest), so do nothing

        float h = 0f;  // left/right
        float v = 0f;  // forwards/backwards

        if (Keyboard.current.wKey.isPressed) v = 1f;
        if (Keyboard.current.sKey.isPressed) v = -1f;
        
        if (Keyboard.current.aKey.isPressed) h = -1f;
        if (Keyboard.current.dKey.isPressed) h = 1f;

        transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime);
    }
}