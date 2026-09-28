using UnityEngine;
using TMPro;

// Said two clocks on the ship. The ship clock ticks normally and the Earth clock
// ticks faster the closer we get to said black hole. That's gravitational time dilation:
// strong gravity slows time down for the ship compared to someone far away on Earth.


public class TimeDilation : MonoBehaviour
{
    public TMP_Text shipClockText;
    public TMP_Text earthClockText;
    public Transform blackHole;
    public Transform ship;

    public float schwarzschildRadius = 50f;  // the black hole's event horizon size in scene units

    private float shipTime = 0f;
    private float earthTime = 0f;
    private bool clocksRunning = false;

    // START, PAUSE, RESET and the ending all use these
    public void StartClocks() { clocksRunning = true; }
    public void StopClocks() { clocksRunning = false; }
    public void ResetClocks()

    {
        shipTime = 0f;
        earthTime = 0f;
        clocksRunning = false;
        UpdateDisplays();
    }

    void Update()
    {
        if (!clocksRunning) return;

        // ship time just counts normally
        shipTime += Time.deltaTime;

        // Earth time counts faster by however strong the dilation is right now
        earthTime += Time.deltaTime * CalculateDilationFactor();

        UpdateDisplays();
    }

    // How many Earth seconds pass for every ship second.
    // This is the real formula for time near a black hole: 1 / sqrt(1 - rs/r)
    // (rs = Schwarzschild radius, r = how far away we are).
    // had quite fun researching and implementing this
    // Far away it's about 1 and it shoots up as r gets close to rs.

    float CalculateDilationFactor()
    {
        if (blackHole == null || ship == null) return 1f;

        float distance = Vector3.Distance(ship.position, blackHole.position);

        // capped at 0.99 so we never divide by zero right at said event horizon
        float ratio = Mathf.Clamp(schwarzschildRadius / distance, 0f, 0.99f);


         // and capped at 100x so the Earth clock doesn't go completely mad
        return Mathf.Min(1f / Mathf.Sqrt(1f - ratio), 100f);
    }

    void UpdateDisplays()
    {
        if (shipClockText != null)
            shipClockText.text = "SHIP: " + FormatTime(shipTime);

        if (earthClockText != null)
        {
            // how many times faster does Earth's clock has been going on average so far
            // (Max stops a divide-by-zero on the very first frame)
            
            float ratio = earthTime > 0 ? earthTime / Mathf.Max(shipTime, 0.001f) : 1f;

            earthClockText.text = "EARTH: " + FormatTime(earthTime)
                + "\n<size=14>(" + ratio.ToString("F1") + "x faster)</size>";
        }
    }

    // turns seconds into mm:ss, e.g. 75 -> "01:15"
    string FormatTime(float seconds)
    {
        int mins = (int)(seconds / 60);
        int secs = (int)(seconds % 60);
        return string.Format("{0:00}:{1:00}", mins, secs);
    }
}

