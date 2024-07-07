using FeralCompany.Modules.SettingsMenu.UI;
using UnityEngine;

namespace FeralCompany.Modules.SettingsMenu;

public class SettingsMenu : MonoBehaviour
{
    internal static SettingsMenu Create() => new GameObject("FeralSettingsMenu").AddComponent<SettingsMenu>();

    internal SettingsMenuUI SettingsUI = null!;

    private void Awake()
    {
        // SettingsUI = Instantiate(Feral.Assets.PrefabSettingsMenu, transform, false).AddComponent<SettingsMenuUI>();
        SettingsUI.Init();
    }
}
