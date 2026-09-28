using UnityEngine;

// This goes on each planet whilst it moves it in a circle around the sun and draws its orbit ring.
// When said sun gets heavier the planet slowly drifts into when it gets lighter it drifts outwards.
// It's a simplified model on purpose or else the real physics would be a lot messier.
public class OrbitingBody : MonoBehaviour
{
    [Header("References")]
    public Transform sun;
    public SunController sunController;

    [Header("Orbit Settings")]

    public float gravityConstant = 2f;       // made-up "G" that just looks good at this scale
    public float selfRotationSpeed = 20f;    // how fast the planet spins on its own axis

    [Header("Mass Reaction (how the orbit responds to sun mass)")]
    [Tooltip("How fast the planet drifts in or out when the sun's mass is different from the default (500). Higher = it gets swallowed or flung out faster. Lower = a slower, gentler drift.")]
    public float radialDriftCoefficient = 0.05f;
    [Tooltip("Closest a planet can get to the sun's centre, so it doesn't shoot out the other side. Anything small near zero is fine.")]
    public float minOrbitRadius = 0.05f;
    [Tooltip("Furthest a planet can drift out when the sun is really light. Just a safety limit.")]
    public float maxOrbitRadius = 1000f;

    [Header("Orbit Line")]
    public LineRenderer orbitLine;
    public int orbitLineSegments = 64;  // more segments = smoother circle

    private float orbitRadius;
    private float orbitAngle;
    private float orbitAngularSpeed;
    // saved starting values so RESET can put everything back
    private float startOrbitRadius;
    private float startOrbitAngle;
    private float startYOffset;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;

        // work out where the planet sits compared to the sun
        Vector3 offset = transform.position - sun.position;
        startYOffset = offset.y;

        // distance to the sun, ignoring height (the 0.01 stops a divide-by-zero later)
        orbitRadius = Mathf.Max(new Vector3(offset.x, 0f, offset.z).magnitude, 0.01f);
        orbitAngle = Mathf.Atan2(offset.z, offset.x);
        startOrbitRadius = orbitRadius;
        startOrbitAngle = orbitAngle;

        // Orbiting speed from the simple gravity formula v = sqrt(G * M / r).
        // Worked out once per planet from its starting distance and it's the reason
        // closer planets go round faster without me tuning each one by hand.
        float orbitSpeedAtStart = Mathf.Sqrt(gravityConstant * sunController.mass / orbitRadius);
        orbitAngularSpeed = orbitSpeedAtStart / orbitRadius;

        if (orbitLine != null) orbitLine.positionCount = orbitLineSegments;
    }

    void Update()

    {
        // simulationSpeed lets me speed up or slow down every planet from one place
        float dt = Time.deltaTime * sunController.simulationSpeed;
        float massDelta = sunController.mass - sunController.defaultMass;

        // heavier sun than default (positive) = planet pulled in
        // lighter sun than default (negative)= planet drifts out into space
        orbitRadius -= radialDriftCoefficient * massDelta * dt;
        orbitRadius  = Mathf.Clamp(orbitRadius, minOrbitRadius, maxOrbitRadius);

        orbitAngle +=  orbitAngularSpeed * dt;

        // turn the angle and radius back into an actual position around the sun
        Vector3 offset = new Vector3(Mathf.Cos(orbitAngle) * orbitRadius, startYOffset, Mathf.Sin(orbitAngle) * orbitRadius);
        transform.position = sun.position + offset;

        transform.Rotate(Vector3.up, selfRotationSpeed * Time.deltaTime, Space.Self);
        DrawOrbitRing(orbitRadius);
    }

    // draws the circle line so you can see the orbit shrinking or growing
    void DrawOrbitRing(float radius)
    {
        if (orbitLine == null) return;
        for (int i = 0; i < orbitLineSegments; i++)
        {
            float angle = ((float)i / orbitLineSegments) * Mathf.PI * 2f;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            orbitLine.SetPosition(i, sun.position + new Vector3(x, 0, z));
        }
        orbitLine.loop = true;  // this joins the last point back to the first
    }

    // called by said SolarSystemManager when RESET is pressed
    public void ResetOrbit()
    {
        orbitRadius = startOrbitRadius;
        orbitAngle = startOrbitAngle;
        transform.position = startPosition;
    }
}