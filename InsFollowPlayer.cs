using UnityEngine;

public class InsFollowPlayer : MonoBehaviour
{
    public GameObject menu;
    public float distanceToPlayer = 2.0f; // Default distance
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        PositionMenu();
    }

    void Update()
    {
        if (menu.activeSelf)
        {
            PositionMenu();
        }
    }

    private void PositionMenu()
    {
        // Set position in front of the player
        menu.transform.position = cameraTransform.position + cameraTransform.forward * distanceToPlayer;

        // Make the menu face the player
        menu.transform.rotation = Quaternion.LookRotation(menu.transform.position - cameraTransform.position);
    }
}
