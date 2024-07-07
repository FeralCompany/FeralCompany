using System.Collections.Generic;
using FeralCompany.Core.UI;
using FeralCompany.Modules.Map.Pointers;
using TMPro;
using UnityEngine;

namespace FeralCompany.Modules.Map;

public sealed class MapUI : FeralUI
{
    internal string TargetName
    {
        get => _targetNameText.text;
        set => _targetNameText.text = value;
    }

    internal Quaternion CompassRotation
    {
        get => _compassTransform.transform.rotation;
        set => _compassTransform.transform.rotation = value;
    }

    internal Rect CameraRect { get; private set; }

    private TMP_Text _targetNameText = null!;
    private RectTransform _compassTransform = null!;
    private Transform _pointers = null!;

    private List<MapPointer>? _currentPointers;

    protected override void AfterAwake()
    {
        _targetNameText = Window.Find("Panel/Target").GetComponent<TMP_Text>();
        _compassTransform = Window.Find("Compass").GetComponent<RectTransform>();
        _pointers = Window.Find("Pointers");
    }

    protected override void OnInit()
    {
        Feral.Events.OnPointersCreated += OnPointersCreated;
        Feral.Settings.Map.Scale.ChangeEvent += UpdateScale;
        UIScale = Feral.Settings.Map.Scale;
    }

    protected override void OnBaseDestroy()
    {
        Feral.Events.OnPointersCreated -= OnPointersCreated;
        Feral.Settings.Map.Scale.ChangeEvent -= UpdateScale;
    }

    private void OnPointersCreated(IReadOnlyList<MapPointer> pointers)
    {
        foreach (var pointer in pointers)
            pointer.PointerObject.transform.SetParent(_pointers, false);

        _currentPointers = new List<MapPointer>(pointers);
        _currentPointers.Sort();

        _pointers.gameObject.SetActive(false);
        for (var i = 0; i < _currentPointers.Count; i++)
        {
            var pointer = _currentPointers[i];
            pointer.PointerObject.transform.SetSiblingIndex(i);
        }

        _pointers.gameObject.SetActive(transform);
    }

    protected override void HandleScaleUpdate(float width, float height)
    {
        var position = WindowRect.position;
        CameraRect = new Rect(
            (position.x - width) / Screen.width,
            (position.y - height) / Screen.height,
            width / Screen.width,
            height / Screen.height
        );

        if (IsOpen)
            Feral.Map.Camera.rect = CameraRect;
    }

    protected override bool CanOpen()
    {
        return Feral.Settings.Map.Enable;
    }
}
