using UnityEngine;

// This is hooked up to the RESET button in the solar system scene.
// Puts the sun back to its normal mass and every planet back where it started.
public class SolarSystemManager : MonoBehaviour
{
    public SunController sunController;
    
    public OrbitingBody[] planets;  // drag all 8 planets in here

    public void ResetSolarSystem()
    {
        sunController.ResetMass();
        foreach (OrbitingBody p in planets)
        {
            p.ResetOrbit();
        }
    }
}