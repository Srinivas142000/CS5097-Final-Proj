using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Matcreator : MonoBehaviour
{
    public GameObject targetObject, buttonPrefab;
    public Transform buttonParent;
    public string materialsFolder = "WallMaterialsDum";

    void Start()
    {
        var materials = Resources.LoadAll<Material>(materialsFolder);

        if (materials.Length == 0)
        {
            Debug.LogError($"No materials found in Resources/{materialsFolder}");
            return;
        }

        foreach (var mat in materials)
            CreateButton(mat);
    }

    void CreateButton(Material mat)
    {
        var btn = Instantiate(buttonPrefab, buttonParent);

        var label = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (label) label.text = mat.name;

        var img = btn.GetComponent<Image>();
        if (mat.mainTexture is Texture2D tex)
        {
            img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
        }

        btn.GetComponent<Button>().onClick.AddListener(() => ApplyMaterial(mat));
    }

    void ApplyMaterial(Material mat)
    {
        if (!targetObject) return;

        var renderer = targetObject.GetComponent<Renderer>();
        if (renderer)
        {
            renderer.material = mat;
            Debug.Log($"Material set to {mat.name} on {targetObject.name}");
        }
    }
}
