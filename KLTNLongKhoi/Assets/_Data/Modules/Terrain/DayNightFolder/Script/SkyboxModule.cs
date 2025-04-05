using UnityEngine;

public class SkyboxModule : ModuleBase
{
    [SerializeField]
    private Gradient skyColor;
    [SerializeField]
    private Gradient horizonColor;

    public override void UpdateModule(float intensity)
    {
        RenderSettings.skybox.SetColor(" _SkyTint ", skyColor.Evaluate(intensity));
        RenderSettings.skybox.SetColor(" _GroundColor ", horizonColor.Evaluate(intensity));
    }
    
}
