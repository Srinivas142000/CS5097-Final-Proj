using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace

public class ColorPicker : MonoBehaviour
{
    public Slider redSlider; // Slider for Red value
    public Slider greenSlider; // Slider for Green value
    public Slider blueSlider; // Slider for Blue value

    public TextMeshProUGUI redValueText; // Text to display Red slider value
    public TextMeshProUGUI greenValueText; // Text to display Green slider value
    public TextMeshProUGUI blueValueText; // Text to display Blue slider value

    public Button applyButton; // Button to apply the color
    public WallCreator wallCreator; // Reference to the WallCreator script

    void Start()
    {
        // Ensure sliders and texts are assigned
        if (redSlider == null || greenSlider == null || blueSlider == null ||
            redValueText == null || greenValueText == null || blueValueText == null ||
            applyButton == null || wallCreator == null)
        {
            Debug.LogError("Please assign all references in the Inspector.");
            return;
        }

        // Set slider ranges (0 to 255 for RGB values)
        redSlider.minValue = 0;
        redSlider.maxValue = 255;
        greenSlider.minValue = 0;
        greenSlider.maxValue = 255;
        blueSlider.minValue = 0;
        blueSlider.maxValue = 255;

        // Add listeners to update text when sliders change
        redSlider.onValueChanged.AddListener(value => UpdateRedText(value));
        greenSlider.onValueChanged.AddListener(value => UpdateGreenText(value));
        blueSlider.onValueChanged.AddListener(value => UpdateBlueText(value));

        // Add listener to apply button
        applyButton.onClick.AddListener(ApplyColor);

        // Initialize text values
        UpdateRedText(redSlider.value);
        UpdateGreenText(greenSlider.value);
        UpdateBlueText(blueSlider.value);
    }

    void UpdateRedText(float value)
    {
        redValueText.text = $"R: {Mathf.RoundToInt(value)}"; // Update TextMeshProUGUI text
    }

    void UpdateGreenText(float value)
    {
        greenValueText.text = $"G: {Mathf.RoundToInt(value)}"; // Update TextMeshProUGUI text
    }

    void UpdateBlueText(float value)
    {
        blueValueText.text = $"B: {Mathf.RoundToInt(value)}"; // Update TextMeshProUGUI text
    }

    void ApplyColor()
    {
        if (wallCreator != null && wallCreator.createdWall != null)
        {
            Renderer wallRenderer = wallCreator.createdWall.GetComponent<Renderer>();
            if (wallRenderer != null && wallRenderer.material.HasProperty("_Color"))
            {
                // Get RGB values from sliders and create a new color
                float r = redSlider.value / 255f; // Normalize to 0-1 range
                float g = greenSlider.value / 255f; // Normalize to 0-1 range
                float b = blueSlider.value / 255f; // Normalize to 0-1 range

                Color selectedColor = new Color(r, g, b);

                // Apply the color to the wall material
                wallRenderer.material.color = selectedColor;

                Debug.Log($"Applied color: R={redSlider.value}, G={greenSlider.value}, B={blueSlider.value}");
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
