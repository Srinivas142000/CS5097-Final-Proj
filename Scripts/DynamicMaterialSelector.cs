using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DynamicMaterialSelector : MonoBehaviour
{
    public GameObject targetObject; // Assigning the wall object here due to wall being generarted in the WallCreator script at runtime
    public GameObject buttonPrefab; // UI button with Image component for showing what kind of material will be applied on the wall
    public Transform buttonParent; // Panel content should be referenced since we are pulling in materials dynamically and assigning them to buttons

    private List<Material> materials = new List<Material>(); // Initialize a list of materials for pulling them in from Resources

    void Start()
    {
        // Load all materials from Resources/WallMaterials
        Material[] loadedMaterials = Resources.LoadAll<Material>("WallMaterials");
        materials.AddRange(loadedMaterials);

        // Assign them to button prefab
        foreach (Material mat in materials)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonParent);

            // Set button texture preview
            if (mat.mainTexture != null)
            {
                Texture2D tex = mat.mainTexture as Texture2D;
                newButton.GetComponent<Image>().sprite = Sprite.Create(tex,
                    new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
            }

            // Handles click liostender for changing the material upon button click
            newButton.GetComponent<Button>().onClick.AddListener(() =>
                ChangeMaterial(mat));
        }
    }

    // Changes material of the wall object to selected material

    void ChangeMaterial(Material newMaterial)
    {
        if (targetObject != null)
        {
            Renderer renderer = targetObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = newMaterial;
            }
        }
    }
}
