using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FeralCompany.Modules.Map.Pointers;

public sealed class PointerController : MonoBehaviour
{
    private TMP_Text _distance = null!;
    private RectTransform _pointer = null!;
    private Image _icon = null!;

    internal bool Active
    {
        get => transform.gameObject.activeSelf;
        set => transform.gameObject.SetActive(value);
    }

    internal float Distance
    {
        get => int.Parse(_distance.text);
        set => _distance.text = $"{Mathf.RoundToInt(value)}";
    }

    internal float Pointer
    {
        get => _pointer.transform.rotation.eulerAngles.z;
        set => _pointer.transform.rotation = Quaternion.Euler(0f, 0f, value);
    }

    internal Sprite Icon
    {
        get => _icon.sprite;
        set => _icon.sprite = value;
    }

    private void Awake()
    {
        _distance = transform.Find("Distance").GetComponent<TMP_Text>();
        _pointer = transform.Find("Pointer").GetComponent<RectTransform>();
        _icon = transform.Find("Icon").GetComponent<Image>();
    }
}
