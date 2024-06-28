using FeralCompany.Core.UI;
using TMPro;
using UnityEngine;

namespace FeralCompany.Modules.Map.UI;

public class MapUI : FeralUI
{
    internal Rect CameraRect { get; private set; }

    internal string Name
    {
        set => _targetText.text = value;
    }

    private RectTransform _window = null!;
    private TMP_Text _targetText = null!;

    protected override void OnInit()
    {
        _window = Window.Find("Window").gameObject.GetComponent<RectTransform>();
        _targetText = Window.Find("Window/Panel/Target").GetComponent<TMP_Text>();
    }

    internal void ChangeMapScale(float scaleFactor)
    {
        CurrentCanvasScale = OriginalCanvasScale * scaleFactor;

        var position = _window.position;
        var size = _window.sizeDelta;
        var width = size.x * CurrentCanvasScale;
        var height = size.y * CurrentCanvasScale;

        CameraRect = new Rect(
            (position.x - width) / Screen.width,
            (position.y - height) / Screen.height,
            width / Screen.width,
            height / Screen.height
        );
    }

    protected override bool CanOpen() => Feral.Settings.Map.Enable;
}
