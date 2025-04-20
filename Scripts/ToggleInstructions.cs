using UnityEngine;

public class ToggleCanvases : MonoBehaviour
{
    public GameObject canvas1; // Assign in Inspector
    public GameObject canvas2; // Assign in Inspector

    public void Toggle()
    {
        bool isCanvas1Active = canvas1.activeSelf;

        // Toggle the active state
        canvas1.SetActive(!isCanvas1Active);
        canvas2.SetActive(isCanvas1Active);
    }
}
