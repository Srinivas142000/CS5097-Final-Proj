using UnityEngine;

public class ResetWall : MonoBehaviour
{
    public WallCreator wallCreator; // Reference to the WallCreator script

    void Start()
    {
        // Find the WallCreator script in the scene if not assigned
        if (wallCreator == null)
        {
            wallCreator = FindObjectOfType<WallCreator>();
            if (wallCreator == null)
            {
                Debug.LogError("WallCreator script not found in the scene. Please assign one.");
            }
        }
    }

    // Method to reset the wall to its default state
    public void ResetWallToDefault()
    {
        if (wallCreator != null && wallCreator.createdWall != null)
        {
            Renderer renderer = wallCreator.createdWall.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Reset material to default
                if (wallCreator.defaultWallMaterial != null)
                {
                    renderer.material = wallCreator.defaultWallMaterial;
                    Debug.Log($"Reset wall material to default: {wallCreator.defaultWallMaterial.name}");
                }

                // Reset texture (if applicable)
                renderer.material.mainTexture = null;
                Debug.Log("Reset wall texture to none.");

                // Reset color (if applicable)
                if (renderer.material.HasProperty("_Color"))
                {
                    renderer.material.color = Color.white; // Default color
                    Debug.Log("Reset wall color to white.");
                }
            }
        }
        else
        {
            Debug.LogError("Wall or WallCreator is not properly assigned.");
        }
    }

    // Example usage: Call this method when a button is clicked or an event triggers
    public void OnResetButtonClicked()
    {
        ResetWallToDefault();
    }
}
