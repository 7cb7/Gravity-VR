using UnityEngine;
using UnityEngine.InputSystem;

// This handles said jumping and falling in the Moon/Earth scene.
// Unity's normal physics gravity is the same everywhere, thus we do our own here,
// that way LocationManager can swap between Moon and Earth gravity.
public class JumpGravity : MonoBehaviour
{
    public float gravity = -1.62f;  // Moon gravity in m/s², LocationManager changes this to -9.81 on Earth
    public float jumpForce = 3f;    // same push every jump so said gravity is the only thing that changes

    [Header("VR Controller Jump Input")]
    public InputActionReference jumpAction;  // the A button (XRHands/Jump)

    [Header("Jump Forgiveness" )]
    [Tooltip("How long (in seconds) a jump press is remembered, and how long after leaving the ground you can still jump. Stops jumps getting 'eaten'.")]
    
    public float jumpGraceTime = 0.15f;


    private float verticalVelocity = 0f;     // how fast we're going up (+) or down (-)
    
    private CharacterController controller;

    // when we last touched the ground and when jump was last pressed
    private float lastGroundedTime = -10f;
    private float lastJumpPressTime = -10f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // said actions don't do anything until they're switched on
        if (jumpAction != null && jumpAction.action != null)

            jumpAction.action.Enable();
    }



    void Update()

    {
        // space bar for testing in the Editor whilst A button in the headset or can click jump button button
        bool keyboardJump = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        
        bool controllerJump = jumpAction != null && jumpAction.action != null && jumpAction.action.WasPressedThisFrame();

        // don't jump straight away just remember that it was pressed
        if (keyboardJump || controllerJump)

            lastJumpPressTime = Time.time;

        if (controller.isGrounded)
        {
            lastGroundedTime = Time.time;

            // push down into the floor so isGrounded doesn't flicker.
            // (was -0.5, but on a fast Mac that push was so tiny Unity sometimes
            // thought one'd left the ground which is what was eating the jumps)
            if (verticalVelocity < 0) verticalVelocity = -1f;
        }
        else
        {
            // in the air so gravity slows us down and pulls one back.
            // weaker gravity = slower pull = higher, floatier jump
            verticalVelocity += gravity * Time.deltaTime;
        }

        // jump if we were on the ground a moment ago AND jump was pressed a moment ago.
        // the small window means a press never gets lost on a bad frame
        bool recentlyGrounded = Time.time - lastGroundedTime <= jumpGraceTime;
        bool recentlyPressed = Time.time - lastJumpPressTime <= jumpGraceTime;

        // verticalVelocity <= 0 stops a sneaky double jump while we're still going up
        if (recentlyGrounded && recentlyPressed && verticalVelocity <= 0f)
        {
            verticalVelocity = jumpForce;

            // use them up so one press = one jump
            lastJumpPressTime = -10f;
            lastGroundedTime = -10f;
        }

        controller.Move(new Vector3(0, verticalVelocity * Time.deltaTime, 0));
    }

    // hooked up to the JUMP button on the canvas.
    // same trick as the keys it just remembers said press and Update does the jump
    public void JumpFromButton()
    {
        lastJumpPressTime = Time.time;
    }
}