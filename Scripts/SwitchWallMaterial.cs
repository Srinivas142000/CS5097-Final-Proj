using UnityEngine;
using TMPro;

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

    // ✅ Load all materials from the specified folder inside Resources
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

    // ✅ Populate the dropdown with material names
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

    // ✅ Handle material selection from the dropdown
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
