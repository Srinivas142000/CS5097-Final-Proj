using UnityEngine;
using UnityEngine.UI;

public class ToggleCanvas : MonoBehaviour
{
    public GameObject instructionsCanvas; // Assign in Inspector
    public GameObject menuCanvas; // Assign in Inspector

    public void ToggleCanvases()
    {
        // Toggle the active state of both canvases
        bool isInstructionsActive = instructionsCanvas.activeSelf;

        instructionsCanvas.SetActive(!isInstructionsActive);
        menuCanvas.SetActive(isInstructionsActive);
    }
}
