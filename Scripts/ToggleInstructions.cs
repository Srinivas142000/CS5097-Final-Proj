using UnityEngine;

// Script no longer being used can be sxrapped

public class ToggleCanvases : MonoBehaviour
{
    public GameObject InstructionsCanvas, menuCanvas;

    public void toggle()
    {
        bool isShowing = InstructionsCanvas.activeSelf;

        InstructionsCanvas.SetActive(!isShowing);
        menuCanvas.SetActive(isShowing);
    }
}
