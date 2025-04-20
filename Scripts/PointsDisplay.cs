using UnityEngine;
using TMPro;

// This is for inscreen debugging of the position of the object in 3D space. It will display the X, Y, and Z coordinates of the object in a TextMeshProUGUI component. The text will always face the OVR camera rig to ensure visibility in VR.

public class PointsDisplay : MonoBehaviour
{
    public TextMeshProUGUI positionText; // Assign this in the Inspector

    void Update()
    {
        if (positionText != null)
        {
            // Update position text
            Vector3 pos = transform.position;
            positionText.text = $"X={pos.x:F2}, Y={pos.y:F2}, Z={pos.z:F2}";

            // Ensure the text always faces the OVR camera rig
            GameObject ovrCamera = GameObject.Find("[BuildingBlock] Camera Rig/TrackingSpace/CenterEyeAnchor");
            if (ovrCamera != null)
            {
                transform.rotation = Quaternion.LookRotation(transform.position - ovrCamera.transform.position);
            }
        }
    }
}
