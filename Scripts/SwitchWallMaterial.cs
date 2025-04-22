using UnityEngine;
using TMPro;


// Change materials using a dropdown menu.
public class SwitchWallMaterial : MonoBehaviour
{
    public Material[] availableMaterials; // Array to store loaded materials
    public string materialsFolder = "WallMaterials"; // Folder to load materials from
    public TMP_Dropdown materialDropdown; // TMP Dropdown for material selection
    public WallCreator wallCreator; // Reference to WallCreator

    // Start is called before the first frame update
    void Start()
    {
        LoadMaterialsFromFolder(materialsFolder);

        // Populate the dropdown if assigned
        if (materialDropdown != null)
        {
            PopulateDropdown();
            materialDropdown.onValueChanged.AddListener(OnMaterialSelected);
        }
        else
        {
            Debug.LogWarning("Material Dropdown is not assigned in the Inspector.");
        }
    }

    void LoadMaterialsFromFolder(string folderName)
    {
        availableMaterials = Resources.LoadAll<Material>(folderName);

        if (availableMaterials.Length == 0)
        {
            Debug.LogWarning("No materials found in Resources/" + folderName);
        }
        else
        {
            Debug.Log(availableMaterials.Length + " materials loaded from Resources/" + folderName);
        }
    }

    void PopulateDropdown()
    {
        materialDropdown.ClearOptions(); // Clear any existing options

        // Add new material names as options
        foreach (Material mat in availableMaterials)
        {
            materialDropdown.options.Add(new TMP_Dropdown.OptionData(mat.name)); // Corrected for TMP_Dropdown
        }

        materialDropdown.RefreshShownValue(); // Update the dropdown display
    }

    // Handle material selection from the dropdown
    public void OnMaterialSelected(int index)
    {
        if (index >= 0 && index < availableMaterials.Length)
        {
            Material selectedMaterial = availableMaterials[index];
            Debug.Log("Selected Material: " + selectedMaterial.name);
            if (wallCreator != null)
            {
                // Just apply the material, no need for SetDefaultMaterial if not needed
                wallCreator.ApplyMaterial(selectedMaterial);
            }
            else
            {
                Debug.LogWarning("WallCreator is not assigned in the Inspector.");
            }
        }
        else
        {
            Debug.LogError("Invalid material index: " + index);
        }
    }


}


using UnityEngine;
using TMPro;

// Dropdown to change the wall material
public class SwitchWallMaterial : MonoBehaviour
{
    public Material[] availableMaterials;
    public string materialsFolder = "WallMaterials"; // Folder name inside Resources
    public TMP_Dropdown materialDropdown;
    public WallCreator wallCreator;

    void Start()
    {
        // Load all materials from the specified folder in Resources
        LoadMaterialsFromFolder(materialsFolder);

        if (materialDropdown != null)
        {
            PopulateDropdown();
            materialDropdown.onValueChanged.AddListener(OnMaterialSelected);
        }
        else
        {
            Debug.LogWarning("Dropdown not hooked up — check inspector!");
        }
    }

    void LoadMaterialsFromFolder(string folder)
    {
        availableMaterials = Resources.LoadAll<Material>(folder);

        if (availableMaterials == null || availableMaterials.Length == 0)
        {
            Debug.LogWarning($"No materials found in Resources/{folder} — folder?");
        }
        else
        {
            Debug.Log($"Loaded {availableMaterials.Length} material(s) from Resources/{folder}");
        }
    }

    void PopulateDropdown()
    {
        materialDropdown.ClearOptions();

        var materialNames = new System.Collections.Generic.List<string>();
        foreach (var mat in availableMaterials)
        {
            materialNames.Add(mat.name);
        }

        materialDropdown.AddOptions(materialNames);
        materialDropdown.RefreshShownValue();
    }

    // material selection from the dropdown
    void OnMaterialSelected(int index)
    {
        if (index < 0 || index >= availableMaterials.Length)
        {
            Debug.LogError($"Material index out of bounds: {index}");
            return;
        }

        var selectedMat = availableMaterials[index];
        Debug.Log($"Applying material: {selectedMat.name}");

        if (wallCreator != null)
        {
            wallCreator.ApplyMaterial(selectedMat);
        }
        else
        {
            Debug.LogWarning("WallCreator not set. Can't apply material.");
        }
    }
}
