using FeralCompany.Modules.Map.Targets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace FeralCompany.Modules.Map;

public sealed class FeralMap : MonoBehaviour
{
    private static int TargetCount => Feral.Globals.MapTargets.Count;

    internal MapTarget Target => Feral.Globals.MapTargets[_targetIndex];
    internal Camera Camera { get; private set; } = null!;

    private Light _light = null!;
    private MapUI _ui = null!;
    private int _targetIndex;

    private void Awake()
    {
        var cameraObj = new GameObject("Feral_MapCamera");
        cameraObj.transform.SetParent(transform);
        Camera = cameraObj.AddComponent<Camera>();
        Camera.enabled = false;
        Camera.orthographic = true;
        Camera.orthographicSize = Feral.Settings.Map.Zoom;

        var cameraData = cameraObj.AddComponent<HDAdditionalCameraData>();
        cameraData.customRenderingSettings = true;
        cameraData.renderingPathCustomFrameSettings.SetEnabled(FrameSettingsField.Volumetrics, false);
        cameraData.renderingPathCustomFrameSettingsOverrideMask.mask[(int)FrameSettingsField.Volumetrics] = true;

        _light = new GameObject("Feral_MapLight").AddComponent<Light>();
        _light.transform.SetParent(transform);
        _light.range = 100f;
        _light.type = LightType.Directional;
        _light.color = Color.white;
        _light.colorTemperature = 6_500f;
        _light.useColorTemperature = true;

        _ui = Instantiate(Feral.Assets.PrefabFeralMap, transform).AddComponent<MapUI>();

        UpdateCullingMask(Feral.Settings.Map.CullingMask);

        _ui.Init();
        _ui.OpenEvent += OnOpenMap;
        _ui.CloseEvent += OnCloseMap;

        IngamePlayerSettings.Instance.playerInput.actions.FindAction("SwitchItem").performed += OnMouseScroll;
    }

    private void Start()
    {
        Feral.Bindings.ToggleMap.performed += _ =>
        {
            Feral.Settings.Map.Enable.Value = !Feral.Settings.Map.Enable.Value;
            if (Feral.Settings.Map.Enable)
                _ui.Open();
            else
                _ui.Close();
        };

        Feral.Bindings.CycleMapTarget.performed += _ => NextTarget();

        RenderPipelineManager.beginCameraRendering += BeginCameraRendering;
        RenderPipelineManager.endCameraRendering += EndCameraRendering;
        Feral.Settings.Map.CullingMask.ChangeEvent += UpdateCullingMask;
        Feral.Settings.Map.Zoom.ChangeEvent += UpdateZoom;
    }

    private static void OnMouseScroll(InputAction.CallbackContext context)
    {
        if (Feral.Bindings.Shift.IsPressed()
            && Feral.Bindings.Alt.IsPressed())
        {
            var value = context.ReadValue<float>();
            var current = Feral.Settings.Map.Scale.Value;
            var multiplier = current / 100;
            var newValue = Mathf.Clamp(current - value * multiplier, 1, 5);
            Feral.Settings.Map.Scale.Value = newValue;
            return;
        }

        if (Feral.Bindings.Alt.IsPressed())
        {
            var value = context.ReadValue<float>();
            var current = Feral.Settings.Map.Zoom.Value;
            var multiplier = current / 100 * 8;
            var newValue = Mathf.Clamp(current - value * multiplier, 1, 100);
            Feral.Settings.Map.Zoom.Value = newValue;
        }
    }

    private void OnOpenMap()
    {
        Camera.rect = _ui.CameraRect;
        Camera.enabled = true;
    }

    private void OnCloseMap()
    {
        Camera.enabled = false;
    }

    private const float NorthAtan2 = 1.570796f;
    private void Update()
    {
        ValidateTarget();
        foreach (var pointer in Feral.CurrentRound.Pointers)
            pointer.UpdateFor(Target);

        _light.intensity = Target.IsInFacility ? Feral.Settings.Map.InternalLight : Feral.Settings.Map.ExternalLight;

        _light.transform.position = Target.Position + Vector3.up * 30f;
        _light.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        var angle = NorthAtan2 - Mathf.Atan2(Target.Forward.x, Target.Forward.z);
        angle *= Mathf.Rad2Deg;
        angle = (angle + 360) % 360;
        _ui.CompassRotation = Quaternion.Euler(0f, 0f, -angle);
    }

    private void LateUpdate()
    {
        var target = Target;
        _ui.TargetName = Target.Name;

        Camera.transform.position = target.CameraPosition;
        Camera.transform.eulerAngles = target.CameraRotation;
        Camera.nearClipPlane = target.NearClipPlane;
    }

    private void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= BeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= EndCameraRendering;
        Feral.Settings.Map.CullingMask.ChangeEvent -= UpdateCullingMask;
        Feral.Settings.Map.Zoom.ChangeEvent -= UpdateZoom;
        IngamePlayerSettings.Instance.playerInput.actions.FindAction("SwitchItem").performed -= OnMouseScroll;
    }

    private void BeginCameraRendering(ScriptableRenderContext _, Camera camera) => _light.enabled = Camera == camera;
    private void EndCameraRendering(ScriptableRenderContext _, Camera camera) => _light.enabled = false;

    private void UpdateCullingMask(int mask) => Camera.cullingMask = mask;
    private void UpdateZoom(float zoom) => Camera.orthographicSize = zoom;

    private void NextTarget()
    {
        _targetIndex++;
        if (_targetIndex < 0 || _targetIndex >= TargetCount)
            _targetIndex = 0;
    }

    private void ValidateTarget()
    {
        if (TargetCount == 0)
            return;

        if (_targetIndex < 0)
            _targetIndex = 0;

        if (_targetIndex >= TargetCount)
            _targetIndex = TargetCount - 1;

        var startIndex = _targetIndex;
        while (!Target.ValidateTarget())
        {
            _targetIndex++;
            if (_targetIndex >= TargetCount)
                _targetIndex = 0;

            if (_targetIndex == startIndex)
                return;
        }
    }
}
