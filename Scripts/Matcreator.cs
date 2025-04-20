using UnityEngine;
using UnityEngine.UI;
using TMPro; // Required for TextMeshPro

public class Matcreator : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject buttonPrefab;
    public Transform buttonParent;
    public string materialsFolder = "WallMaterialsDum";

    void Start()
    {
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
        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>(); // Find TextMeshPro component
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

        // Add click event listener
        Button buttonComponent = newButton.GetComponent<Button>();
        buttonComponent.onClick.AddListener(() => ChangeMaterial(material));
    }

    void ChangeMaterial(Material newMaterial)
    {
        if (targetObject != null)
        {
            Renderer renderer = targetObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = newMaterial;
                Debug.Log($"Changed material of {targetObject.name} to {newMaterial.name}");
            }
            else
            {
                Debug.LogError("Target object does not have a Renderer component.");
            }
        }
    }
}
