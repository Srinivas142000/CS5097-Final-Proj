using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ItemLoader : MonoBehaviour
{
    // Generate materials in editor and assign them to the prefab folder in Resource
    [Header("UI")]
    public GameObject buttonPrefab;
    public Transform buttonParent;

    // Pull in materials from Resources/Items
    [Header("Prefabs")]
    public string prefabFolder = "Items";
    public GameObject buildingBlockTemplatePrefab;

    // The distance at which we want the objects to spawn
    [Header("Spawn Settings")]
    public float spawnOffset = 2.0f;

    private GameObject selectedPrefab;
    private WallCreator wallCreator;

    // Create a dictionary of prefabs and their sprite to avoid duplication
    private Dictionary<GameObject, Sprite> prefabSpriteCache = new Dictionary<GameObject, Sprite>();

    void Start()
    {
        // Load all materials from Resources/Items
        GameObject[] prefabs = Resources.LoadAll<GameObject>(prefabFolder);

        if (prefabs.Length == 0)
        {
            Debug.LogError($"No prefabs found in Resources/{prefabFolder}");
            return;
        }

        // Find WallCreator Object during runtime
        wallCreator = FindObjectOfType<WallCreator>();
        if (wallCreator == null)
        {
            Debug.LogWarning("WallCreator not found in the scene. Scaling will default to 1.");
        }
        // Apply prefabs to button prefab
        foreach (GameObject prefab in prefabs)
        {
            CreatePrefabButton(prefab);
        }
    }

    void CreatePrefabButton(GameObject prefab)
    {
        // Instantiate the button prefab and set its parent
        GameObject newButton = Instantiate(buttonPrefab, buttonParent);

        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
            buttonText.text = prefab.name;

        Image buttonImage = newButton.GetComponent<Image>();
        Sprite prefabSprite = GetPrefabSprite(prefab);

        if (prefabSprite != null)
        {
            buttonImage.sprite = prefabSprite;
            buttonImage.type = Image.Type.Simple;
        }

        Button buttonComponent = newButton.GetComponent<Button>();
        buttonComponent.onClick.AddListener(() => SelectPrefab(prefab));
    }

    Sprite GetPrefabSprite(GameObject prefab)
    {
        if (prefabSpriteCache.ContainsKey(prefab))
            return prefabSpriteCache[prefab];
        return null;
    }

    void SelectPrefab(GameObject prefab)
    {
        selectedPrefab = prefab;
        if (selectedPrefab != null)
        {
            SpawnPrefabWithVisualSwap();
        }
    }

    // After selecting the prefab we need to assign the meta building blocks to the prefab

    void SpawnPrefabWithVisualSwap()
    {
        if (selectedPrefab == null || buildingBlockTemplatePrefab == null)
        {
            Debug.LogError("Missing prefab or building block template.");
            return;
        }

        // Spawn position
        Transform cam = Camera.main.transform;
        Vector3 forwardOffset = cam.forward * spawnOffset;
        Vector3 upOffset = Vector3.up * 0.5f;
        Vector3 spawnPosition = cam.position + forwardOffset + upOffset;

        GameObject block = Instantiate(buildingBlockTemplatePrefab, spawnPosition, Quaternion.identity);

        // Copy mesh data
        MeshFilter sourceMeshFilter = selectedPrefab.GetComponentInChildren<MeshFilter>();
        MeshRenderer sourceRenderer = selectedPrefab.GetComponentInChildren<MeshRenderer>();

        if (sourceMeshFilter == null || sourceRenderer == null)
        {
            Debug.LogWarning($"{selectedPrefab.name} is missing MeshFilter or MeshRenderer.");
            return;
        }

        MeshFilter blockMeshFilter = block.GetComponentInChildren<MeshFilter>();
        MeshRenderer blockRenderer = block.GetComponentInChildren<MeshRenderer>();

        if (blockMeshFilter != null && blockRenderer != null)
        {
            blockMeshFilter.sharedMesh = sourceMeshFilter.sharedMesh;
            blockRenderer.sharedMaterials = sourceRenderer.sharedMaterials;
        }

        // Set scale based on wall
        if (wallCreator != null)
        {
            block.transform.localScale = wallCreator.transform.localScale * 0.5f;
        }
        else
        {
            block.transform.localScale = Vector3.one * 0.5f;
        }

        Debug.Log($"Spawned '{selectedPrefab.name}' using Building Block at {spawnPosition} with scale based on WallCreator.");
    }
}
