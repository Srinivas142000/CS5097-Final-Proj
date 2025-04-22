using UnityEngine;
using UnityEngine.UI;
using TMPro;

// lets you pick a color using sliders and apply it to the generated wall

public class ColorPicker : MonoBehaviour
{
    public Slider redSli, greenSli, blueSli;
    public TextMeshProUGUI redValue, greenValue, blueValue;
    public Button applyButton;
    public WallCreator wallCreator;

    void Start()
    {
        // quick null check on all required refs
        if (!redSli || !greenSli || !blueSli || 
            !redValue || !greenValue || !blueValue ||
            !applyButton || !wallCreator)
        {
            Debug.LogError("Missing references — make sure all fields are assigned.");
            return;
        }

        // sliders go from 0 to 255
        SetupSlider(redSli);
        SetupSlider(greenSli);
        SetupSlider(blueSli);

        // link slider changes to UI text updates
        redSli.onValueChanged.AddListener(val => UpdateText(val, "red"));
        greenSli.onValueChanged.AddListener(val => UpdateText(val, "green"));
        blueSli.onValueChanged.AddListener(val => UpdateText(val, "blue"));

        applyButton.onClick.AddListener(ApplyColor);

        // init display values
        UpdateText(redSli.value, "red");
        UpdateText(greenSli.value, "green");
        UpdateText(blueSli.value, "blue");
    }

    void SetupSlider(Slider s)
    {
        s.minValue = 0;
        s.maxValue = 255;
    }

    void UpdateText(float value, string color)
    {
        int rounded = Mathf.RoundToInt(value);
        switch (color.ToLower())
        {
            case "red":
                redValue.text = $"R: {rounded}";
                break;
            case "green":
                greenValue.text = $"G: {rounded}";
                break;
            case "blue":
                blueValue.text = $"B: {rounded}";
                break;
            default:
                Debug.LogWarning("Unknown color label");
                break;
        }
    }

    void ApplyColor()
    {
        if (wallCreator == null || wallCreator.createdWall == null)
        {
            Debug.LogError("Wall object missing or not generated yet.");
            return;
        }

        var renderer = wallCreator.createdWall.GetComponent<Renderer>();
        if (renderer && renderer.material.HasProperty("_Color"))
        {
            float r = redSli.value / 255f;
            float g = greenSli.value / 255f;
            float b = blueSli.value / 255f;

            renderer.material.color = new Color(r, g, b);

            Debug.Log($"Applied color → R: {redSli.value}, G: {greenSli.value}, B: {blueSli.value}");
        }
        else
        {
            Debug.LogError("Material doesn’t support color change (_Color not found)");
        }
    }
}
