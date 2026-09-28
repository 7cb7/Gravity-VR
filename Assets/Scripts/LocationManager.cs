using UnityEngine;

// this runs ssaid Moon <-> Earth switching. When you travel it swaps the environment,
// the info panel, the travel button, the gravity and the sky, then moves you to the new spawn point.
public class LocationManager : MonoBehaviour
{
    // lets other scripts reach this one with LocationManager.Instance
    public static LocationManager Instance;

    public enum Location { Moon, Earth }
    public Location currentLocation = Location.Moon;

    [Header("Environment Parents - drag the two parent GameObjects here")]
    public GameObject moonEnvironment;
    public GameObject earthEnvironment;

    [Header("Info Panels - drag the two info panel GameObjects here")]
    public GameObject moonInfoPanel;
    public GameObject earthInfoPanel;

    [Header("Travel Buttons - drag the two travel button GameObjects here")]
    public GameObject goToEarthButton;
    public GameObject goToMoonButton;

    [Header("Spawn Points - empty GameObjects marking where the player lands")]
    public Transform moonSpawnPoint;
    public Transform earthSpawnPoint;

    [Header("Player References")]
    public CharacterController playerController;
    public JumpGravity playerGravity;

    [Header("Gravity Values")]
    public float moonGravity = -1.62f;   // real Moon value m/s^2
    public float earthGravity = -9.81f;  // real Earth value m/s^2

    [Header("Sky")]
    public Camera playerCamera;
    public Color moonSkyColor = Color.black;  // no atmosphere on the Moon thus the sky's black

    void Awake()
    {
        // only ever want one of these so get rid of any extras
        if (Instance == null) Instance = this;

        else Destroy(gameObject);


    }

    void Start()
    {
        // always start on the Moon
        SetLocation(Location.Moon, true);
    }

    public void TravelTo(Location newLocation)
    {
        if (newLocation == currentLocation) return;  // already there
        SetLocation(newLocation, true);
    }

    // simple versions for the buttons since On Click can't pass an enum

    public void TravelToEarth() { TravelTo(Location.Earth); }

     public void TravelToMoon() { TravelTo(Location.Moon); }

    void SetLocation(Location loc, bool teleportPlayer)
    {
        currentLocation = loc;
        bool goingToMoon = (loc == Location.Moon);

        // show one world whilst hide the other
        moonEnvironment.SetActive(goingToMoon);
        earthEnvironment.SetActive(!goingToMoon);

        // matching info panel
        if (moonInfoPanel != null) moonInfoPanel.SetActive(goingToMoon);
        if (earthInfoPanel != null) earthInfoPanel.SetActive(!goingToMoon);

        // on the Moon you get "Go to Earth" whilst on Earth you get "Go to Moon"
        if (goToEarthButton != null) goToEarthButton.SetActive(goingToMoon) ;

        if (goToMoonButton != null) goToMoonButton.SetActive(!goingToMoon);

         // the actual physics bit - swap the gravity the player feels
        if (playerGravity != null)
            playerGravity.gravity = goingToMoon ? moonGravity : earthGravity;

        // black sky on the Moon whilst normal skybox on Earth
        if (playerCamera != null)
        {
            if (goingToMoon)
            {
                playerCamera.clearFlags = CameraClearFlags.SolidColor;
                playerCamera.backgroundColor = moonSkyColor;
            }

             else
            {
                playerCamera.clearFlags = CameraClearFlags.Skybox;
            }
        }

        if (teleportPlayer)
            TeleportPlayer(goingToMoon ? moonSpawnPoint : earthSpawnPoint);
    }

    void TeleportPlayer(Transform spawnPoint)
    {
        if (playerController == null || spawnPoint == null) return;

        // said  CharacterController ignores position changes while it's on,
        // so switch it off, move the player, then switch it back on
        
        playerController.enabled = false;
        playerController.transform.position = spawnPoint.position;
        playerController.transform.rotation = spawnPoint.rotation;
        playerController.enabled = true;
    }
}