using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TextureManager : MonoBehaviour
{
    public GameObject buttonPrefab;        // Button prefab with an Image component
    public Transform buttonParent;         // UI Panel to hold the buttons
    public WallCreator wallCreator;        // Reference to the WallCreator script
    public string textureFolder = "WallMaterials"; // Folder in Resources

    private List<Texture> loadedTextures = new List<Texture>();

    void Start()
    {
        LoadTextures();
        GenerateTextureButtons();
    }

    // Load textures from the Resources/Textures folder
    void LoadTextures()
    {
        Texture[] textures = Resources.LoadAll<Texture>(textureFolder);
        if (textures.Length == 0)
        {
            Debug.LogWarning("No textures found in Resources/Textures folder.");
            return;
        }

        loadedTextures.AddRange(textures);
    }

    // Generate buttons dynamically for each texture
    void GenerateTextureButtons()
    {
        if (loadedTextures.Count == 0) return;

        foreach (Texture texture in loadedTextures)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, buttonParent);
            buttonObj.name = texture.name;

            // Set the button label
            buttonObj.GetComponentInChildren<Text>().text = texture.name;

            // Set button image preview
            Image buttonImage = buttonObj.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = Sprite.Create(
                    (Texture2D)texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
            }

            // Add button click listener
            Texture selectedTexture = texture;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => ApplyTextureToWall(selectedTexture));
        }
    }

    // Apply the selected texture to the wall
    void ApplyTextureToWall(Texture texture)
    {
        if (wallCreator != null)
        {
            wallCreator.ApplyTexture(texture);
        }
        else
        {
            Debug.LogError("WallCreator reference is missing in TextureManager.");
        }
    }
}
