using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ItemLoader : MonoBehaviour
{
    [Header("UI")]
    public GameObject buttonPrefab;
    public Transform buttonParent;

    [Header("Prefabs")]
    public string prefabFolder = "Items";
    public GameObject buildingBlockTemplatePrefab;

    [Header("Spawn Settings")]
    public float spawnOffset = 2.0f;

    GameObject selectedPrefab;
    WallCreator wallCreator;
    Dictionary<GameObject, Sprite> prefabSpriteCache = new();

    void Start()
    {
        var prefabs = Resources.LoadAll<GameObject>(prefabFolder);
        if (prefabs.Length == 0)
        {
            Debug.LogError($"No prefabs found in Resources/{prefabFolder}");
            return;
        }

        wallCreator = FindObjectOfType<WallCreator>();
        if (!wallCreator)
            Debug.LogWarning("WallCreator not found – fallback scale will be used.");

        foreach (var prefab in prefabs)
            CreatePrefabButton(prefab);
    }

    void CreatePrefabButton(GameObject prefab)
    {
        var btn = Instantiate(buttonPrefab, buttonParent);
        var label = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (label) label.text = prefab.name;

        var img = btn.GetComponent<Image>();
        var sprite = GetPrefabSprite(prefab);
        if (sprite)
        {
            img.sprite = sprite;
            img.type = Image.Type.Simple;
        }

        btn.GetComponent<Button>().onClick.AddListener(() => SelectPrefab(prefab));
    }

    Sprite GetPrefabSprite(GameObject prefab)
    {
        return prefabSpriteCache.TryGetValue(prefab, out var sprite) ? sprite : null;
    }

    void SelectPrefab(GameObject prefab)
    {
        selectedPrefab = prefab;
        if (selectedPrefab)
            SpawnPrefabWithVisualSwap();
    }

    void SpawnPrefabWithVisualSwap()
    {
        if (!selectedPrefab || !buildingBlockTemplatePrefab)
        {
            Debug.LogError("Missing selected prefab or block template.");
            return;
        }

        var cam = Camera.main.transform;
        var spawnPos = cam.position + cam.forward * spawnOffset + Vector3.up * 0.5f;

        var block = Instantiate(buildingBlockTemplatePrefab, spawnPos, Quaternion.identity);

        var srcFilter = selectedPrefab.GetComponentInChildren<MeshFilter>();
        var srcRenderer = selectedPrefab.GetComponentInChildren<MeshRenderer>();

        if (!srcFilter || !srcRenderer)
        {
            Debug.LogWarning($"{selectedPrefab.name} is missing MeshFilter or MeshRenderer.");
            return;
        }

        var blockFilter = block.GetComponentInChildren<MeshFilter>();
        var blockRenderer = block.GetComponentInChildren<MeshRenderer>();

        if (blockFilter && blockRenderer)
        {
            blockFilter.sharedMesh = srcFilter.sharedMesh;
            blockRenderer.sharedMaterials = srcRenderer.sharedMaterials;
        }

        block.transform.localScale = wallCreator ? wallCreator.transform.localScale * 0.5f : Vector3.one * 0.5f;

        Debug.Log($"Spawned '{selectedPrefab.name}' at {spawnPos} with adjusted scale.");
    }
}
