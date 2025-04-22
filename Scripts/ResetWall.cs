using UnityEngine;

// resets wall to its original material, texture, and color

public class ResetWall : MonoBehaviour
{
    public WallCreator wallCreator;

    void Start()
    {
        // auto-assign if missing
        if (wallCreator == null)
        {
            wallCreator = FindObjectOfType<WallCreator>();
            if (wallCreator == null)
            {
                Debug.LogError("No WallCreator found in scene – assign manually.");
            }
        }
    }

    public void ResetWallToDefault()
    {
        if (wallCreator == null || wallCreator.createdWall == null)
        {
            Debug.LogError("Missing reference – check wallCreator or wall object.");
            return;
        }

        var renderer = wallCreator.createdWall.GetComponent<Renderer>();
        if (renderer == null) return;

        // reset to default material if available
        if (wallCreator.defaultWallMaterial != null)
        {
            renderer.material = wallCreator.defaultWallMaterial;
            Debug.Log("Wall material reset to default.");
        }

        // clear texture
        renderer.material.mainTexture = null;

        // reset color
        if (renderer.material.HasProperty("_Color"))
        {
            renderer.material.color = Color.white;
        }
    }

    // hook this up to a button if needed
    public void OnResetButtonClicked()
    {
        ResetWallToDefault();
    }
}
