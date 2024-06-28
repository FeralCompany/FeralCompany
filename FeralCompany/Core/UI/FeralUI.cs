using System;
using UnityEngine;

namespace FeralCompany.Core.UI;

public abstract class FeralUI : MonoBehaviour
{
    protected Transform Window { get; private set; } = null!;
    protected Canvas Canvas { get; private set; } = null!;
    protected float OriginalCanvasScale { get; private set; }

    protected float CurrentCanvasScale
    {
        get => Canvas.scaleFactor;
        set => Canvas.scaleFactor = value;
    }

    internal event Action? OpenEvent;
    internal event Action? CloseEvent;

    internal bool IsOpen { get; private set; }

    internal void Init()
    {
        Window = transform.Find("Container/Window");
        if (Window)
            Window.gameObject.SetActive(false);

        Canvas = GetComponent<Canvas>();
        OriginalCanvasScale = Canvas.scaleFactor;

        OpenEvent += OnOpen;
        CloseEvent += OnClose;

        OnInit();
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
    }

    internal void Open()
    {
        if (IsOpen || !Window)
            return;
        IsOpen = true;

        Window.gameObject.SetActive(true);
        OpenEvent?.Invoke();
    }

    internal void Close()
    {
        if (!IsOpen || !Window)
            return;

        IsOpen = false;
        Window.gameObject.SetActive(false);
        CloseEvent?.Invoke();
    }

    protected abstract void OnInit();

    protected virtual void OnOpen() { }
    protected virtual void OnClose() { }

    protected virtual bool CanOpen()
    {
        return true;
    }
}
