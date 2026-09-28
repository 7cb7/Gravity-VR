using UnityEngine;
using UnityEngine.UI;
using TMPro;

// controls said sun's mass from the slider and makes the sun look heavier or lighter to match
// (bigger, glowier and brighter as the mass goes up). Said planets read the mass from here.
public class SunController : MonoBehaviour
{
    [Header("Mass Settings")]
    public float mass = 500f;
    public float minMass = 100f;
    public float maxMass = 2000f;
    public float defaultMass = 500f;  // the "normal" mass~ planets don't drift at all at this value

    [Header("Mass Smoothing")]
    public float massSmoothSpeed = 2.5f;  // how quickly said mass catches up with the slider
    private float targetMass;             // where the slider is, whilst actual mass eases towards it

    [Header("Simulation Speed (shared by all planets)")]
    public float simulationSpeed = 1.5f;

    [Header("Visuals That React To Mass")]

    public Transform sunMesh;
    public Transform glowSphere;
    public Light sunLight;


    [Header("Visual Scaling Ranges (multipliers, not actual sizes)")]
    public float minSunScale = 0.85f;
    public float maxSunScale = 1.3f;
    public float minGlowScale = 0.8f;
    public float maxGlowScale = 2.2f;

    public  float minLightIntensity = 1f;
    public float maxLightIntensity = 4f;

    [Header("UI")]
    public TMP_Text massReadoutText;
    public Slider massSlider;

    // the sun and glow sizes at the start so the multipliers above scale from these
    private Vector3 sunBaseScale;
    private Vector3 glowBaseScale;

    void Start()
    {
        mass = defaultMass;
        targetMass = defaultMass;

        if (sunMesh != null) sunBaseScale = sunMesh.localScale;
        if (glowSphere != null) glowBaseScale = glowSphere.localScale;
        ApplyVisuals();
    }

    void Update()
    {
        // this ease said mass towards the slider instead of jumping straight there,
        // so the planets react smoothly when you drag it fast
        if (!Mathf.Approximately(mass, targetMass))
        {
            mass = Mathf.Lerp(mass, targetMass, Time.deltaTime * massSmoothSpeed);

            // close enough, just snap to it (Lerp would otherwise creep forever)

            if (Mathf.Abs(mass - targetMass) < 0.5f) mass = targetMass;

            ApplyVisuals();
        }
    }

    // the slider's On Value Changed calls this
    public void SetMass(float newMass)
    {
        targetMass = Mathf.Clamp(newMass, minMass, maxMass);
    }

    // used by RESET as it snaps everything straight back with no easing
    public void ResetMass()
    {
        mass = defaultMass;
        targetMass = defaultMass;
        if (massSlider != null) massSlider.value = defaultMass;
        ApplyVisuals();
    }

    void ApplyVisuals()
    {
        // turns the mass into a 0-1 value (0 = lightest, 1 = heaviest)
        float t = Mathf.InverseLerp(minMass, maxMass, mass);

        if (sunMesh != null)

            sunMesh.localScale = sunBaseScale * Mathf.Lerp(minSunScale, maxSunScale, t);
        if (glowSphere != null)
            glowSphere.localScale = glowBaseScale * Mathf.Lerp(minGlowScale, maxGlowScale, t);
        if (sunLight != null)
            sunLight.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, t);



        // "F0" = no decimal places
        if (massReadoutText != null)
            massReadoutText.text = "Relative Mass Value: " + mass.ToString("F0");
    }
}