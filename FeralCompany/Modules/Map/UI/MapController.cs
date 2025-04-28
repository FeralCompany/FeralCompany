using System;
using TMPro;
using UnityEngine;

namespace FeralCompany.Modules.Map.UI
{
    public class MapController : MonoBehaviour
    {
#pragma warning disable CS8618
        [SerializeField] private TextMeshProUGUI target;
        [SerializeField] private RectTransform pointers;
        [SerializeField] private RectTransform compass;
        [SerializeField] private RectTransform compassNorth;
        [SerializeField] private RectTransform compassEast;
        [SerializeField] private RectTransform compassSouth;
        [SerializeField] private RectTransform compassWest;
#pragma warning restore CS8618

        private MapPointerController[] _currentPointers = Array.Empty<MapPointerController>();

        internal string TargetName
        {
            get => target.text;
            set => target.text = value;
        }

        internal float CompassAngle
        {
            get => compass.rotation.eulerAngles.z;
            set
            {
                compass.rotation = Quaternion.Euler(0f, 0f, value);
                compassNorth.transform.rotation = Quaternion.Euler(0f, 0f, -value);
                compassEast.transform.rotation = Quaternion.Euler(0f, 0f, -value);
                compassSouth.transform.rotation = Quaternion.Euler(0f, 0f, -value);
                compassWest.transform.rotation = Quaternion.Euler(0f, 0f, -value);
            }
        }

        internal void AddNewPointers(MapPointerController[] newPointers)
        {
            ClearCurrentPointers();
            _currentPointers = newPointers;
            for (var i = 0; i < _currentPointers.Length; i++)
            {
                var pointer = _currentPointers[i];
                pointer.transform.SetParent(pointers);
                pointer.transform.SetSiblingIndex(i);
            }
        }

        internal void ClearCurrentPointers()
        {
            foreach (var pointer in _currentPointers)
                Destroy(pointer.gameObject);
        }
    }
}
