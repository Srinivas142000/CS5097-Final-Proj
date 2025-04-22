using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DynamicMaterialSelector : MonoBehaviour
{
    public GameObject targetObject, buttonPrefab;
    public Transform buttonParent;

    List<Material> materials = new();

    void Start()
    {
        var loadedMaterials = Resources.LoadAll<Material>("WallMaterials");
        materials.AddRange(loadedMaterials);

        foreach (var mat in materials)
        {
            var btn = Instantiate(buttonPrefab, buttonParent);

            if (mat.mainTexture is Texture2D tex)
            {
                btn.GetComponent<Image>().sprite = Sprite.Create(
                    tex,
                    new Rect(0, 0, tex.width, tex.height),
                    new Vector2(0.5f, 0.5f)
                );
            }

            btn.GetComponent<Button>().onClick.AddListener(() => ApplyMaterial(mat));
        }
    }

    void ApplyMaterial(Material mat)
    {
        if (!targetObject) return;

        var renderer = targetObject.GetComponent<Renderer>();
        if (renderer) renderer.material = mat;
    }
}
