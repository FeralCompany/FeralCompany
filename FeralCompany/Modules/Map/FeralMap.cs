using FeralCompany.Modules.Map.UI;
using FeralCompany.Utils.LayerMask;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace FeralCompany.Modules.Map;

public class FeralMap : MonoBehaviour
{
    internal static FeralMap Create() => new GameObject("FeralMinimap").AddComponent<FeralMap>();

    internal Camera Camera { get; private set; } = null!;
    internal Light Light { get; private set; } = null!;
    internal MapUI MapUI { get; private set; } = null!;

    private void Awake()
    {
        var cameraObject = new GameObject("Feral_MapCamera");
        cameraObject.transform.SetParent(transform);

        Camera = cameraObject.AddComponent<Camera>();
        Camera.enabled = false;
        Camera.orthographic = true;

        Light = new GameObject("Feral_MapLight").AddComponent<Light>();
        Light.transform.SetParent(transform);
        Light.range = 100;
        Light.intensity = 15f;
        Light.type = LightType.Directional;
        Light.color = Color.white;
        Light.colorTemperature = 6500;
        Light.useColorTemperature = true;
        Light.gameObject.layer = LayerMask.NameToLayer("HelmetVisor");

        Feral.Player.gameplayCamera.cullingMask =
            Mask.Remove(Feral.Player.gameplayCamera.cullingMask, Masks.Unused1);

        var hdCameraData = cameraObject.AddComponent<HDAdditionalCameraData>();
        hdCameraData.customRenderingSettings = true;
        hdCameraData.renderingPathCustomFrameSettings.SetEnabled(FrameSettingsField.Volumetrics, false);
        hdCameraData.renderingPathCustomFrameSettingsOverrideMask.mask[(int)FrameSettingsField.Volumetrics] = true;

        gameObject.AddComponent<MapTarget>();

        MapUI = Instantiate(Feral.Assets.PrefabFeralMap, transform).AddComponent<MapUI>();
        MapUI.Init();
        MapUI.OpenEvent += OnMapOpen;
        MapUI.CloseEvent += OnMapClose;

        Feral.Settings.Map.CullingMask.ChangeEvent += UpdateCullingMask;
        Feral.Settings.Map.Scale.ChangeEvent += ChangeMapScale;

        UpdateCullingMask(Feral.Settings.Map.CullingMask);
        ChangeMapScale(Feral.Settings.Map.Scale);
    }

    private void Update()
    {
        Camera.orthographicSize = Feral.Settings.Map.Zoom;
        Light.transform.position = Camera.transform.position + Vector3.up * 30f;
    }

    private void OnMapOpen()
    {
        Camera.enabled = true;
        Camera.rect = MapUI.CameraRect;
    }

    private void OnMapClose()
    {
        Camera.enabled = false;
    }

    private void OnDestroy()
    {
        Feral.Settings.Map.CullingMask.ChangeEvent -= UpdateCullingMask;
        Feral.Settings.Map.Scale.ChangeEvent -= ChangeMapScale;
    }

    private void ChangeMapScale(float scaleFactor)
    {
        MapUI.ChangeMapScale(scaleFactor);
        if (MapUI.IsOpen)
            Camera.rect = MapUI.CameraRect;
    }

    private void UpdateCullingMask(Masks[] masks)
    {
        Camera.cullingMask = Mask.Compress(masks);
    }
}
