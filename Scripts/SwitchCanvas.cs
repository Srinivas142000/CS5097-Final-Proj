using UnityEngine;

public class SwitchCanvas : MonoBehaviour
{
    public GameObject currentCanvas; // Assign current canvas in Inspector
    public GameObject newCanvas;     // Assign the new canvas in Inspector

    // Call this method when button is clicked
    public void SwitchToNewCanvas()
    {
        if (currentCanvas != null)
        {
            currentCanvas.SetActive(false); // Disable current canvas
        }

        if (newCanvas != null)
        {
            newCanvas.SetActive(true); // Enable new canvas
        }
        else
        {
            Debug.LogWarning("New canvas is not assigned in the Inspector!");
        }
    }
}
