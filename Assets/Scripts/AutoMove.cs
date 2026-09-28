using UnityEngine;

// this shall flies said ship towards the black hole once START is pressed.
// it sits on the Ship object and the VR rig rides along inside it.
public class AutoMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float stopDistance = 60f;   // ship stops once it's this close to the black hole to not have continues journey with no stop
    public Transform blackHole;
    public bool isMoving = false;

    private bool hasStopped = false;   // true once reached of the black hole
    private bool hasStarted = false;   // true once START has been pressed

    [Header("Clocks - drag GameManager in here")]
    public TimeDilation timeDilation;

    // The VR controller sometimes sends one trigger pull as two clicks I have come to realize so...
    // For a toggle like pause that means pause + unpause instantly, so it looks

    // like nothing happened. Remembering when we last paused lets us ignore the double.

    private float lastPauseTime = -1f;

    void Update()
    {
        if (!isMoving || hasStopped) return;

        // close enough to the black hole? then stop for good
        if (blackHole != null)


        {
            float distance = Vector3.Distance(transform.position, blackHole.position);
            if (distance <= stopDistance) { isMoving = false; hasStopped = true; return; }
        }

        // this otherwise keep flying forward (deltaTime keeps the speed the same on any frame rate)
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    // hooked up to the START button
    public void StartJourney()
    {
        isMoving = true;
        hasStopped = false;

         hasStarted = true;
    }

    // hooked up to said PAUSE button press once to pause and again to carry on
    public void PauseJourney()
    {
        // this second click within 0.3 seconds? that's the double click then ignore it
        if (Time.unscaledTime - lastPauseTime < 0.3f) return;
        lastPauseTime = Time.unscaledTime;

        // nothing to pause if we haven't started yet or already arrived
        if (!hasStarted || hasStopped) return;

        isMoving = !isMoving;

        // the clocks have to pause with the ship, otherwise time keeps ticking whilst we sit still not the point of the learning experience
        if (timeDilation != null)
        {
            if (isMoving) timeDilation.StartClocks();
            else timeDilation.StopClocks();
        }
    }

    

    // hooked up to the RESET button thus it puts the ship back at the start
    public void ResetJourney()
    {

        isMoving = false;
        hasStopped = false;
        hasStarted = false;

        transform.position = Vector3.zero;
    }
}