using UnityEngine;
using UnityEngine.UI;


// toggleCanvas script to manage the visibility of two canvases in Unity.
// this was written to toggle between two set canvases but used to toggle a single canvas 
public class ToggleCanvas : MonoBehaviour
{
    public GameObject instructionsCanvas, menuCanvas;

    public void ToggleCanvases()
    {
        // Toggle the active state of both canvases
        bool isInstructionsVisible = instructionsCanvas.activeSelf;

        instructionsCanvas.SetActive(!isInstructionsVisible);
        menuCanvas.SetActive(isInstructionsVisible);
    }
}
