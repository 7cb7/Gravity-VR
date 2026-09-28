using UnityEngine;

// Old approach to keep one player rig alive across every scene.
// Not used any more, every scene now has its own copy of the VR rig that I created a prefab of  instead.
// Left in the project in case it's useful later
public class PersistentRig : MonoBehaviour
{
    public static PersistentRig Instance;

    [Header("Drag this same GameObject's own parts into these three slots")]
    public CharacterController characterController;
    public JumpGravity jumpGravity;
    public Camera playerCamera;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // survives scene changes
        }
        else
        {
            // a rig already came with us from the last scene, so remove this duplicate
            Destroy(gameObject);
        }
    }
}