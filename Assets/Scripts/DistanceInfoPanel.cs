using UnityEngine;

// This pops up the second black hole info panel (the event horizon one) partway
// through the journey, then shall hides it again as you get really close.
public class DistanceInfoPanel : MonoBehaviour
{
    public Transform ship;
    public Transform blackHole;
    public GameObject panel;

    public float showAtDistance = 150f;  // panel appears once one is this close
    public float hideAtDistance = 90f;   // and disappears again once one is closer than this



    void Update()
    {
        if (ship == null || blackHole == null || panel == null) return;

         float distance = Vector3.Distance(ship.position, blackHole.position);

        // only show it while we're inside the "window" between the two distances
        bool shouldShow = distance <= showAtDistance && distance > hideAtDistance;

        // only flip it when it actually needs to change, no point doing it every frame
        if (panel.activeSelf != shouldShow)
            panel.SetActive(shouldShow);
    }
}