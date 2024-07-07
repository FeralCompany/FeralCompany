using System;
using UnityEngine;

namespace FeralCompany.Core.UI;

public abstract class FeralUI : MonoBehaviour
{
    protected Transform Window { get; private set; } = null!;
    protected RectTransform WindowRect { get; private set; } = null!;

    internal event Action? OpenEvent;
    internal event Action? CloseEvent;

    internal bool IsOpen { get; private set; }

    protected float UIScale = 1.0f;

    private Canvas _canvas = null!;
    private float _originalCanvasScale;
    private float _currentCanvasScale;

    private void Awake()
    {
        Window = transform.Find("Container/Window");
        if (Window)
        {
            Window.gameObject.SetActive(false);
            WindowRect = Window.gameObject.GetComponent<RectTransform>();
        }

        _canvas = GetComponent<Canvas>();
        _originalCanvasScale = _canvas.scaleFactor;
        _currentCanvasScale = _originalCanvasScale;
        AfterAwake();
    }

    internal void Init()
    {
        OnInit();
        UpdateScale(UIScale);
    }

    protected void Update()
    {
        if (Feral.Player.quickMenuManager.isMenuOpen)
        {
            Close();
            return;
        }

        if (CanOpen())
            Open();

        AfterUpdate();
    }

    internal void Open()
    {
        if (IsOpen || !Window)
            return;
        IsOpen = true;

        Window.gameObject.SetActive(true);

        OnOpen();
        OpenEvent?.Invoke();
    }

    internal void Close()
    {
        if (!IsOpen || !Window)
            return;

        IsOpen = false;
        Window.gameObject.SetActive(false);

        OnClose();
        CloseEvent?.Invoke();
    }

    private void OnDestroy()
    {
        OnBaseDestroy();
    }

    protected abstract void AfterAwake();
    protected abstract void OnInit();

    protected virtual void AfterUpdate() { }

    protected virtual void OnOpen() { }
    protected virtual void OnClose() { }
    protected virtual void OnBaseDestroy() { }

    protected virtual void HandleScaleUpdate(float width, float height) { }

    protected virtual bool CanOpen() => true;

    internal void UpdateScale(float newScale)
    {
        UIScale = newScale;
        _currentCanvasScale = _originalCanvasScale * newScale;
        _canvas.scaleFactor = _currentCanvasScale;

        var width = WindowRect.sizeDelta.x * _currentCanvasScale;
        var height = WindowRect.sizeDelta.y * _currentCanvasScale;
        HandleScaleUpdate(width, height);
    }
}
