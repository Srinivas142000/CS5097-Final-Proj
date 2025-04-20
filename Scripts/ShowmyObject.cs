using UnityEngine;
using UnityEngine.InputSystem; // Required for input handling

public class ShowmyObject : MonoBehaviour
{
    public GameObject originPrefab; // Assign sphere prefab for origin point
    public GameObject secondPrefab; // Assign sphere prefab for second point
    public Transform player; // Reference to the player's head/camera (OVRCameraRig or XR Rig)
    public float customHeight;

    private float spawnDistance = 2.0f; // Distance from the user to spawn objects

    void Update()
    {
        // Check if the Y button (Left Controller) is pressed
        if (OVRInput.GetDown(OVRInput.Button.Four)) // Y button
        {
            SpawnObject(originPrefab);
        }

        // Check if the B button (Right Controller) is pressed
        if (OVRInput.GetDown(OVRInput.Button.Two)) // B button
        {
            SpawnObject(secondPrefab);
        }
    }

    void SpawnObject(GameObject prefab)
    {
        if (prefab == null || player == null) return;

        // Calculate spawn position in front of the player but slightly offset
        Vector3 spawnPosition = player.position + (player.forward * spawnDistance);
        spawnPosition.y = player.position.y + customHeight; // Keep it at user height

        // Instantiate the object at calculated position
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}
