using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace FeralCompany.Modules;

public class FeralNightVision : MonoBehaviour
{
    private Light _nearLight = null!;
    private Light _farLight = null!;

    private void Awake()
    {
        var mapCamera = GameObject.Find("MapCamera").transform;
        transform.SetParent(FeralCompany.Player.gameplayCamera.transform);

        _nearLight = new GameObject("NearLight").AddComponent<Light>();
        _nearLight.transform.SetParent(transform);
        _nearLight.transform.position = mapCamera.position + Vector3.down * 80f;
        _nearLight.range = 70f;
        _nearLight.color = new Color(0.875f, 0.788f, 0.791f, 1);

        _farLight = new GameObject("FarLight").AddComponent<Light>();
        _farLight.transform.SetParent(transform);
        _farLight.transform.position = mapCamera.position + Vector3.down * 30f;
        _farLight.range = 500f;
    }

    private void Update()
    {
        if (FeralCompany.Player.isInsideFactory)
        {
            _nearLight.intensity = FeralCompany.Settings.General.InternalNightVisionIntensity.Value * 100f;
            _farLight.intensity = FeralCompany.Settings.General.InternalNightVisionIntensity.Value * 1100f;
        }
        else
        {
            _nearLight.intensity = FeralCompany.Settings.General.ExternalNightVisionIntensity.Value * 100f;
            _farLight.intensity = FeralCompany.Settings.General.ExternalNightVisionIntensity.Value * 1100f;
        }
    }

    private void Start()
    {
        RenderPipelineManager.beginCameraRendering += BeginCameraRendering;
        RenderPipelineManager.endCameraRendering += EndCameraRendering;
    }

    private void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= BeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= EndCameraRendering;
    }

    private void BeginCameraRendering(ScriptableRenderContext _, Camera camera)
    {
        if (FeralCompany.Player.gameplayCamera != camera) return;

        _farLight.enabled = true;
        _nearLight.enabled = true;
    }

    private void EndCameraRendering(ScriptableRenderContext _, Camera camera)
    {
        _farLight.enabled = false;
        _nearLight.enabled = false;
    }
}
