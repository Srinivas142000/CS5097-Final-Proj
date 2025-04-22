using UnityEngine;
using UnityEngine.InputSystem;

// spawns two points in front of the user using Y - origin and B - top right buttons
// ended up freezing the item loader - keeping for now in case we fix it

public class ShowmyObject : MonoBehaviour
{
    public GameObject originPrefab, secondPrefab;
    public Transform player; // should be the camera/head reference
    public float customHeight = 1.5f;

    float spawnDistance = 2f;

    void Update()
    {
        // press Y (left controller) to spawn origin point
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            Spawn(originPrefab);
        }

        // press B (right controller) to spawn second point
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            Spawn(secondPrefab);
        }
    }

    void Spawn(GameObject prefab)
    {
        if (prefab == null || player == null) return;

        var pos = player.position + player.forward * spawnDistance;
        pos.y = player.position.y + customHeight;

        Instantiate(prefab, pos, Quaternion.identity);
    }
}
