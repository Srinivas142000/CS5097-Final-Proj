using UnityEngine;

// not using this anymore, can probably delete

public class SwitchCanvas : MonoBehaviour
{
    public GameObject currentCanvas, newCanvas;

    public void switchCanvas()
    {
        if (currentCanvas) currentCanvas.SetActive(false);

        if (newCanvas)
            newCanvas.SetActive(true);
        else
            Debug.LogWarning("newCanvas wasn't set – check the Inspector");
    }
}
