using UnityEngine;
using TMPro;

// displays object's position (x, y, z) on a floating UI text in AR
// rotates the label to always face the camera

public class PointsDisplay : MonoBehaviour
{
    public TextMeshProUGUI positionText;

    void Update()
    {
        if (positionText == null) return;

        var pos = transform.position;
        positionText.text = $"X={pos.x:F2}, Y={pos.y:F2}, Z={pos.z:F2}";

        // rotate to face camera
        var cam = GameObject.Find("[BuildingBlock] Camera Rig/TrackingSpace/CenterEyeAnchor");
        if (cam)
        {
            var dir = transform.position - cam.transform.position;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
