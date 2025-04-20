using UnityEngine;
using TMPro;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown optionDropdown; // Dropdown
    public TMP_Text selectedOption; // Text for Dropdown

    void Start()
    {
        // TO identify changes in dropdown
        optionDropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        // Initialize with default value
        selectedOption.text = "Selected: " + optionDropdown.options[optionDropdown.value].text;
    }

    // Method to update text when Dropdown value changes
    void OnDropdownValueChanged(int value)
    {
        selectedOption.text = "Selected: " + optionDropdown.options[value].text;
    }
}
