using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorPicker : MonoBehaviour
{
    public Slider redSli; // Slider for Red value
    public Slider greenSli; // Slider for Green value
    public Slider blueSli; // Slider for Blue value

    public TextMeshProUGUI redValue; // Text to display Red slider value
    public TextMeshProUGUI greenValue; // Text to display Green slider value
    public TextMeshProUGUI blueValue; // Text to display Blue slider value

    public Button applyButton; // Button to apply the color
    public WallCreator wallCreator; // Reference to the WallCreator script

    void Start()
    {
        // Ensure sliders and texts are assigned
        if (redSli == null || greenSli == null || blueSli == null ||
            redValue == null || greenValue == null || blueValue == null ||
            applyButton == null || wallCreator == null)
        {
            Debug.LogError("Please assign all references in the Inspector.");
            return;
        }

        // Set slider ranges (0 to 255 for RGB values)
        redSli.minValue = 0;
        redSli.maxValue = 255;
        greenSli.minValue = 0;
        greenSli.maxValue = 255;
        blueSli.minValue = 0;
        blueSli.maxValue = 255;

        // Add listeners to update text when sliders change
        redSli.onValueChanged.AddListener(value => UpdateRedText(value));
        greenSli.onValueChanged.AddListener(value => UpdateGreenText(value));
        blueSli.onValueChanged.AddListener(value => UpdateBlueText(value));

        // Add listener to apply button to apply color
        applyButton.onClick.AddListener(ApplyColor);

        // Initialize text values with 0
        UpdateText(redSli.value, "red");
        UpdateText(greenSli.value, "green");
        UpdateText(blueSli.value, "blue");
    }

    // a single method that takes a color and updates the text

    void UpdateText(float value, String color)
    {
        switch (color.ToLower())
        {
            case "red":
                redValue.text = $"R: {Mathf.RoundToInt(value)}"; 
                break;
            case "green":
                greenValue.text = $"G: {Mathf.RoundToInt(value)}"; 
                break;
            case "blue":
                blueValue.text = $"B: {Mathf.RoundToInt(value)}";
                break;
            default:
                Debug.LogError("Invalid color specified.");
                break;
        }
    }

    void ApplyColor()
    {
        if (wallCreator != null && wallCreator.createdWall != null)
        {
            Renderer wallRenderer = wallCreator.createdWall.GetComponent<Renderer>(); // Get Wall Generated at runtime
            if (wallRenderer != null && wallRenderer.material.HasProperty("_Color"))
            {
                // Get RGB values from sliders and create a new color
                float r = redSli.value / 255f; // Normalize to 0-1 range
                float g = greenSli.value / 255f; // Normalize to 0-1 range
                float b = blueSli.value / 255f; // Normalize to 0-1 range

                Color selectedColor = new Color(r, g, b);

                // Apply the color to the wall material
                wallRenderer.material.color = selectedColor;

                Debug.Log($"Color Applied is: R={redSli.value}, G={greenSli.value}, B={blueSli.value}");
            }
            else
            {
                Debug.LogError("Wall material does not support '_Color' property.");
            }
        }
        else
        {
            Debug.LogError("No wall created or WallCreator reference is missing.");
        }
    }
}
