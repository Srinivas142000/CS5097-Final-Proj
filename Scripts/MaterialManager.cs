using UnityEngine;

public class MaterialManager
{
    private Material currentMaterial;

    // Constructor to initialize with a material
    public MaterialManager(Material initialMaterial)
    {
        currentMaterial = initialMaterial;
    }

    // Apply a new material dynamically
    public void ApplyMaterial(Material newMaterial)
    {
        if (newMaterial != null)
        {
            currentMaterial = newMaterial;
            Debug.Log("Material applied: " + newMaterial.name);
        }
        else
        {
            Debug.LogError("Material is null. Cannot apply.");
        }
    }

    // Get the current material
    public Material GetCurrentMaterial()
    {
        return currentMaterial;
    }
}
