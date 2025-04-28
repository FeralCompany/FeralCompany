using UnityEngine;

namespace FeralCompany.Modules.Map.UI
{
    public class UIScaler : MonoBehaviour
    {
#pragma warning disable CS8616
        [SerializeField] private RectTransform monitor;
#pragma warning restore CS8616

        internal RectTransform Rect => monitor;
        internal Vector2 Vector2 { get; private set; }
        internal float Width => Vector2.x;
        internal float Height => Vector2.y;

        private Canvas _canvas = null!;
        private float _originalScale;
        private float _currentScale;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _originalScale = _canvas.scaleFactor;
            _currentScale = _originalScale;
        }
    }
}
