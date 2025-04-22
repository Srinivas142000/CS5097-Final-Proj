using UnityEngine;
using TMPro;

// deleting script, dropdown doesnt work with ray controller

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public TMP_Text label;

    void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownChanged);
        label.text = $"Selected: {dropdown.options[dropdown.value].text}";
    }

    void OnDropdownChanged(int index)
    {
        label.text = $"Selected: {dropdown.options[index].text}";
    }
}
