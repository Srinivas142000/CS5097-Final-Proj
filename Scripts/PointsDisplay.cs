using UnityEngine;
using TMPro;

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
