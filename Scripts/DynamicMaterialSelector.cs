using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DynamicMaterialSelector : MonoBehaviour
{
    public GameObject targetObject; // Assign your target object in Inspector
    public GameObject buttonPrefab; // Create a UI Button prefab with Image component
    public Transform buttonParent; // Assign your panel's content area

    private List<Material> materials = new List<Material>();

    void Start()
    {
        // Load all materials from Resources/WallMaterials
        Material[] loadedMaterials = Resources.LoadAll<Material>("WallMaterials");
        materials.AddRange(loadedMaterials);

        // Create buttons dynamically
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

            // Add click handler with material parameter
            newButton.GetComponent<Button>().onClick.AddListener(() =>
                ChangeMaterial(mat));
        }
    }

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
