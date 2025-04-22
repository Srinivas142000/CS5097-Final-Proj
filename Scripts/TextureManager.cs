using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//loads and assigns textures to wall objects

public class TextureManager : MonoBehaviour
{
    public GameObject buttonPrefab; // UI button prefab also has image component
    public Transform buttonParent; // where to spawn the texture buttons
    public WallCreator wallCreator; // needs reference to the script that builds walls
    public string textureFolder = "WallMaterials"; // add material directly in this folder to make it spawn on thee UI of the App

    private List<Texture> loadedTextures = new List<Texture>();

    void Start()
    {
        LoadTextures();
        GenerateTextureButtons();
    }

    // load all textures from Resources/<textureFolder>
    void LoadTextures()
    {
        Texture[] textures = Resources.LoadAll<Texture>(textureFolder);
        if (textures.Length == 0)
        {
            Debug.LogWarning("No textures found in the given Resources folder");
            return;
        }

        loadedTextures.AddRange(textures);
    }

    // create one UI button for each texture we found
    void GenerateTextureButtons()
    {
        if (loadedTextures.Count == 0) return;

        foreach (var tex in loadedTextures)
        {
            GameObject btn = Instantiate(buttonPrefab, buttonParent);
            btn.name = tex.name;

            // label the button with texture name
            var label = btn.GetComponentInChildren<Text>();
            if (label != null)
                label.text = tex.name;

            // try to preview the texture in the button itself
            var img = btn.GetComponent<Image>();
            if (img != null && tex is Texture2D tex2D)
            {
                img.sprite = Sprite.Create(
                    tex2D,
                    new Rect(0, 0, tex2D.width, tex2D.height),
                    new Vector2(0.5f, 0.5f)
                );
            }

            // set up the button to apply that texture
            Texture selectedTex = tex;
            btn.GetComponent<Button>().onClick.AddListener(() => ApplyTextureToWall(selectedTex));
        }
    }

    // tell the wallCreator to apply the texture
    void ApplyTextureToWall(Texture texture)
    {
        if (wallCreator != null)
        {
            wallCreator.ApplyTexture(texture);
        }
        else
        {
            Debug.LogError("WallCreator ref is null - can't assign texture");
        }
    }
}
