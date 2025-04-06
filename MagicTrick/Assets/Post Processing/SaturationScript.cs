using UnityEngine;

[ExecuteInEditMode]
public class ColorGradingEffect : MonoBehaviour
{
    public Material gradingMaterial; // Reference to the material with the custom shader

    // Color Grading
    public Color lift = Color.white;  // Shadows adjustment
    public Color gamma = Color.white; // Midtones adjustment
    public Color gain = Color.white;  // Highlights adjustment

    // Grain
    [Range(0, 1)] public float grainIntensity = 0.2f; // Intensity of the grain
    [Range(1, 10)] public float grainScale = 2.0f;    // Scale of the grain texture

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (gradingMaterial != null)
        {
            // Set shader properties
            gradingMaterial.SetColor("_Lift", lift);
            gradingMaterial.SetColor("_Gamma", gamma);
            gradingMaterial.SetColor("_Gain", gain);
            gradingMaterial.SetFloat("_GrainIntensity", grainIntensity);
            gradingMaterial.SetFloat("_GrainScale", grainScale);

            // Apply shader effect
            Graphics.Blit(src, dest, gradingMaterial);
        }
        else
        {
            Graphics.Blit(src, dest); // Pass through
        }
    }
}
