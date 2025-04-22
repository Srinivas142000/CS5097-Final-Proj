using UnityEngine;
using System.Collections.Generic;

// handles floating menu UIs that follow the player/camera
// first canvas is always shown, others can be toggled
// might remove this as well, since menu following user made people dizzy

public class MenuFollowPlayer : MonoBehaviour
{
    public List<GameObject> canvases;
    public float distanceToPlayer = 4f;
    public float spacing = 1f;

    Transform cam;
    int activeIdx = -1;

    void Start()
    {
        cam = Camera.main.transform;
        SetupCanvases();
    }

    void SetupCanvases()
    {
        if (canvases == null || canvases.Count == 0) return;

        for (int i = 0; i < canvases.Count; i++)
        {
            if (canvases[i] == null) continue;

            // only show the main menu canvas initially
            canvases[i].SetActive(i == 0);
            PositionCanvas(canvases[i], i);
        }
    }

    void PositionCanvas(GameObject canvas, int idx)
    {
        Vector3 offset;

        if (idx == 0)
        {
            offset = cam.forward * distanceToPlayer;
        }
        else
        {
            var closeDist = distanceToPlayer - 1.5f;
            var left = 3f;
            offset = cam.forward * closeDist - cam.right * left;
        }

        Vector3 targetPos = cam.position + offset;
        canvas.transform.position = targetPos;
        canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - cam.position);
    }

    public void ToggleCanvas(int idx)
    {
        if (idx <= 0 || idx >= canvases.Count)
        {
            Debug.LogError($"Invalid canvas index {idx} – only 1 and up are toggleable.");
            return;
        }

        var canvas = canvases[idx];
        if (canvas == null) return;

        // turn off previous if any
        if (activeIdx != -1 && activeIdx != idx && canvases[activeIdx] != null)
        {
            canvases[activeIdx].SetActive(false);
        }

        // same one clicked again → turn off
        if (activeIdx == idx)
        {
            canvas.SetActive(false);
            activeIdx = -1;
            return;
        }

        // show the new one
        canvas.SetActive(true);
        PositionCanvas(canvas, idx);
        activeIdx = idx;
    }
}
