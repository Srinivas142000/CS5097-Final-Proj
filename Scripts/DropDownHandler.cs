using UnityEngine;
using TMPro;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown optionDropdown; // Reference to the Dropdown
    public TMP_Text selectedOptionText; // Reference to the Text to display the option

    // Start is called before the first frame update
    void Start()
    {
        // Add listener to call the method when Dropdown value is changed
        optionDropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        // Initialize with default value
        selectedOptionText.text = "Selected: " + optionDropdown.options[optionDropdown.value].text;
    }

    // Method to update text when Dropdown value changes
    void OnDropdownValueChanged(int value)
    {
        selectedOptionText.text = "Selected: " + optionDropdown.options[value].text;
    }
}
