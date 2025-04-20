using UnityEngine;
using System.Collections.Generic;

public class MenuFollowPlayer : MonoBehaviour
{
    public List<GameObject> canvases; // List of canvases
    public float distanceToPlayer = 4.0f; // Distance from the player for the primary canvas
    public float spacing = 1.0f; // Spacing between canvases
    private Transform cameraTransform;
    private int activeCanvasIndex = -1; // -1 means no secondary canvas is active

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        SetupCanvases();
    }

    private void SetupCanvases()
    {
        if (canvases == null || canvases.Count == 0) return;

        for (int i = 0; i < canvases.Count; i++)
        {
            if (canvases[i] != null)
            {
                canvases[i].SetActive(i == 0); // Only canvas 0 is active initially
                PositionCanvas(canvases[i], i);
            }
        }
    }
    private void PositionCanvas(GameObject canvas, int index)
    {
        Vector3 offset;

        if (index == 0)
        {
            // Menu canvas always at a fixed distance in front
            offset = cameraTransform.forward * distanceToPlayer;
        }
        else
        {
            // Secondary canvas appears closer to the player
            float closerDistance = distanceToPlayer - 1.5f;
            float leftOffset = 3f; // Adjust this to move more/less to the left

            offset = cameraTransform.forward * closerDistance - cameraTransform.right * leftOffset;

        }

        Vector3 targetPosition = cameraTransform.position + offset;
        canvas.transform.position = targetPosition;
        canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - cameraTransform.position);
    }

    public void ToggleCanvas(int index)
    {
        if (index <= 0 || index >= canvases.Count)
        {
            Debug.LogError($"Invalid canvas index {index}. Only indices 1 to N are toggleable.");
            return;
        }

        if (canvases[index] == null) return;

        // Deactivate previously active canvas (other than canvas 0)
        if (activeCanvasIndex != -1 && activeCanvasIndex != index && canvases[activeCanvasIndex] != null)
        {
            canvases[activeCanvasIndex].SetActive(false);
        }

        // Toggle off if the same canvas is clicked again
        if (activeCanvasIndex == index)
        {
            canvases[index].SetActive(false);
            activeCanvasIndex = -1;
            return;
        }

        // Activate the new canvas
        canvases[index].SetActive(true);
        PositionCanvas(canvases[index], index);
        activeCanvasIndex = index;
    }
}
