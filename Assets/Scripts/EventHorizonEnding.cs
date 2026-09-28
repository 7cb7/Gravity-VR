using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;



// this shall ending of the black hole journey. Once the ship gets close enough,
// the clocks stop, the screen fades to black and the "spaghettified" text shows up.
public class EventHorizonEnding : MonoBehaviour
{
    public Transform ship;
    public Transform blackHole;

    public float triggerDistance = 60f;  // this how close before the ending kicks in
    public Image fadeImage;              // a black image that starts fully see-through
    public TMP_Text absorbedText;
    public float fadeDuration = 2f;      // how long the fade to black takes, in seconds
    public TimeDilation timeDilation;

    private bool hasTriggered = false;   // makes sure the ending only plays once

    void Update()
    {
        if (hasTriggered || ship == null || blackHole == null) return;

        float distance = Vector3.Distance (ship.position, blackHole.position);
        if (distance <= triggerDistance)

        {
            hasTriggered = true;
            StartCoroutine(PlayEnding());
        }
    }

    // A coroutine runs a little bit each frame, which is what lets said fade happen gradually
    IEnumerator PlayEnding()
    {
        if (timeDilation != null) timeDilation.StopClocks();

        // slowly turn the fade image from see-through to solid black
        float t = 0f;
        Color c = fadeImage.color;
        while (t < fadeDuration)

        {
            t += Time.deltaTime;
            c.a = Mathf.Clamp01(t / fadeDuration);  // alpha goes 0 -> 1 over the fade
            fadeImage.color = c;
            yield return null;  // wait for the next frame then carry on
        }

        // make sure it ends up fully black
        c.a = 1f;
        fadeImage.color = c;

        if (absorbedText != null)
            absorbedText.gameObject.SetActive(true);

    }

    // this undoes said ending so the journey can be played again
    public void ResetEnding()
    {
        hasTriggered = false;
        StopAllCoroutines();  // this in case the fade is still halfway through, previously forgot this part

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
        if (absorbedText != null)
            absorbedText.gameObject.SetActive(false);
    }
}