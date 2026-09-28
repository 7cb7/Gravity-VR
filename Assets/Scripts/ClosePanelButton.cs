using UnityEngine;

// This goes on the little X button of an info panel.
// This lets the player close the panel so it doesn't hang around blocking the view, which can be anyoing or obstructing
public class ClosePanelButton : MonoBehaviour
{
    // drag in whichever panel this X should hide
    public GameObject panelToClose;

    // this hooked up to the X button's On Click
    public void ClosePanel()
    {
        if (panelToClose != null)
        
            panelToClose.SetActive(false);
    }
}