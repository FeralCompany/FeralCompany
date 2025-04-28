using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FeralCompany.Modules.Map.UI
{
    public class MapPointerController : MonoBehaviour
    {
#pragma warning disable CS8618
        [SerializeField] private TextMeshProUGUI distance;
        [SerializeField] private Image icon;
        [SerializeField] private RectTransform pointer;
#pragma warning restore CS8618

        internal float Distance
        {
            get => int.Parse(distance.text);
            set => distance.text = $"{Mathf.RoundToInt(value)}";
        }

        internal Sprite Icon
        {
            get => icon.sprite;
            set => icon.sprite = value;
        }

        internal float Rotation
        {
            get => pointer.rotation.eulerAngles.z;
            set => pointer.rotation = Quaternion.Euler(0f, 0f, value);
        }
    }
}
