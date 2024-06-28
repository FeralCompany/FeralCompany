using FeralCompany.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FeralCompany.Modules.SettingsMenu.UI;

public class SettingsMenuUI : FeralUI
{
    private TMP_Text _titleLabel = null!;

    private GameObject _tabList = null!;
    private GameObject _contentList = null!;

    protected override void OnInit()
    {
        _titleLabel = Window.Find("Settings/Title/Text").GetComponent<TMP_Text>();

        _tabList = Window.Find("Tabs").gameObject;
        _contentList = Window.Find("Settings/Content").gameObject;

        Window.Find("Settings/Title/Exit").GetComponent<Button>().onClick.AddListener(Close);
    }
}
