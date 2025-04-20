using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq; // Required for LINQ queries

public class MatcreatorGenWall : MonoBehaviour
{
    public WallCreator wallCreator; // Reference to the WallCreator script
    public CustomWallMaker customWallMaker; // Reference to the CustomWallMaker script (if needed)
    public GameObject buttonPrefab;
    public Transform buttonParent;
    public string materialsFolder = "WallMaterials";

    void Start()
    {
        // Find the WallCreator script in the scene if not assigned
        if (wallCreator == null)
        {
            wallCreator = FindObjectOfType<WallCreator>();
            if (wallCreator == null)
            {
                Debug.LogError("WallCreator script not found in the scene. Please add one.");
                return;
            }
        }
        if (customWallMaker == null)
        {
            customWallMaker = FindObjectOfType<CustomWallMaker>();
            if (customWallMaker == null)
            {
                Debug.LogError("CustomWallMaker script not found in the scene. Please add one.");
                return;
            }
        }

        Material[] materials = Resources.LoadAll<Material>(materialsFolder);

        if (materials.Length == 0)
        {
            Debug.LogError($"No materials found in Resources/{materialsFolder}. Please check your folder structure and file types.");
            return;
        }

        foreach (Material mat in materials)
        {
            CreateButtonForMaterial(mat);
        }
    }

    void CreateButtonForMaterial(Material material)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonParent);

        // Set button text to the material name
        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = material.name;
        }
        else
        {
            Debug.LogWarning($"TextMeshProUGUI component not found on button prefab. Ensure it exists as a child.");
        }

        // Set button image to display material texture
        Image buttonImage = newButton.GetComponent<Image>();
        if (material.mainTexture != null)
        {
            Texture2D texture = material.mainTexture as Texture2D;
            if (texture != null)
            {
                buttonImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            }
        }
        else
        {
            Debug.LogWarning($"Material '{material.name}' does not have a main texture.");
        }

        // Add click event listener to change the wall's material
        Button buttonComponent = newButton.GetComponent<Button>();
        buttonComponent.onClick.AddListener(() => ChangeWallMaterial(material));
    }

    void ChangeWallMaterial(Material newMaterial)
    {
        if (wallCreator != null)
        {
            wallCreator.ApplyMaterial(newMaterial); // Call ApplyMaterial function in WallCreator
            Debug.Log($"Changed wall material to {newMaterial.name}");
        }
        if (customWallMaker != null)
        {
            customWallMaker.ApplyMaterial(newMaterial); // Call ApplyMaterial function in CustomWallMaker if needed
            Debug.Log($"Changed wall material to {newMaterial.name} using CustomWallMaker");
        }
        else
        {
            Debug.LogError("WallCreator script is not assigned.");
        }
    }
}
