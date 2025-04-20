using UnityEngine;
using UnityEngine.InputSystem;// Required for Meta Quest input
public class MenuController : MonoBehaviour
{
    public WallCreator wc;
    public GameObject menuCanvas; // Assign your Menu Canvas in the Inspector

    private void Update()
    {
        // Check for A or X button press
        if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three))
        {
            ToggleMenu();
        }

        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            wc.CreateWall();
        }
    }

    void ToggleMenu()
    {
        menuCanvas.SetActive(!menuCanvas.activeSelf);
    }
}
