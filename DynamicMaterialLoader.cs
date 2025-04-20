using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DynamicMaterialLoader : MonoBehaviour
{
    public GameObject buttonPrefab;             // Assign a Button prefab in the Inspector
    public GameObject targetObject;             // Object to apply the material
    public Transform buttonParent;              // Parent to hold buttons (Canvas Panel)
    public string materialsFolder = "WallMaterials"; // Folder in Resources

    private List<Material> loadedMaterials = new List<Material>();

    void Start()
    {
        LoadMaterials();
        GenerateButtons();
    }

    // Load materials from Resources/Materials folder
    void LoadMaterials()
    {
        Material[] materials = Resources.LoadAll<Material>(materialsFolder);
        if (materials.Length == 0)
        {
            Debug.LogWarning("No materials found in Resources/Materials folder.");
            return;
        }

        foreach (Material mat in materials)
        {
            loadedMaterials.Add(mat);
        }
    }

    // Generate buttons dynamically
    void GenerateButtons()
    {
        if (loadedMaterials.Count == 0) return;

        float spacing = 10f; // Spacing between buttons
        int buttonCount = loadedMaterials.Count;

        GridLayoutGroup grid = buttonParent.GetComponent<GridLayoutGroup>();
        if (grid == null)
        {
            Debug.LogError("GridLayoutGroup is missing on buttonParent. Add it for automatic spacing.");
            return;
        }

        // Set grid spacing and layout dynamically
        grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        grid.constraintCount = 2; // Arrange in 2 rows if possible
        grid.spacing = new Vector2(spacing, spacing);
        grid.cellSize = new Vector2(100, 100); // Button size

        // Instantiate buttons
        for (int i = 0; i < buttonCount; i++)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, buttonParent);
            buttonObj.name = loadedMaterials[i].name;
            buttonObj.GetComponentInChildren<Text>().text = loadedMaterials[i].name;

            // Set button material preview
            Image buttonImage = buttonObj.GetComponent<Image>();
            if (buttonImage != null && loadedMaterials[i].mainTexture != null)
            {
                buttonImage.sprite = Sprite.Create(
                    (Texture2D)loadedMaterials[i].mainTexture,
                    new Rect(0, 0, loadedMaterials[i].mainTexture.width, loadedMaterials[i].mainTexture.height),
                    new Vector2(0.5f, 0.5f)
                );
            }

            // Add button click listener
            int index = i;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => ApplyMaterialToObject(index));
        }
    }

    // Apply selected material to the target object
    void ApplyMaterialToObject(int index)
    {
        if (targetObject != null && loadedMaterials[index] != null)
        {
            Renderer objRenderer = targetObject.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                objRenderer.material = loadedMaterials[index];
                Debug.Log("Material " + loadedMaterials[index].name + " applied to " + targetObject.name);
            }
            else
            {
                Debug.LogError("Target object does not have a Renderer component.");
            }
        }
        else
        {
            Debug.LogWarning("Target object or material is missing.");
        }
    }
}
