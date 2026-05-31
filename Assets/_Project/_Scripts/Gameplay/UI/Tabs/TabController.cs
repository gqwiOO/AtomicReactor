using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.UI.Tabs
{
    [Serializable]
    public class TabEntry
    {
        public TabButton Button;
        public List<GameObject> Content;
    }

    public class TabController : MonoBehaviour
    {
        [SerializeField] private List<TabEntry> _tabs;
        [SerializeField] private int _defaultTabIndex;

        private TabEntry _currentTab;

        private void Start()
        {
            foreach (var tab in _tabs)
                tab.Button.OnClicked += Button_OnClicked;

            if (_tabs.Count > 0)
                SelectTab(_tabs[Mathf.Clamp(_defaultTabIndex, 0, _tabs.Count - 1)]);
        }

        private void OnDestroy()
        {
            foreach (var tab in _tabs)
                tab.Button.OnClicked -= Button_OnClicked;
        }

        public void SelectTab(int index)
        {
            if (index < 0 || index >= _tabs.Count) return;
            SelectTab(_tabs[index]);
        }

        private void Button_OnClicked(TabButton button)
        {
            TabEntry entry = _tabs.Find(t => t.Button == button);
            if (entry != null)
                SelectTab(entry);
        }

        private void SelectTab(TabEntry entry)
        {
            if (_currentTab == entry) return;

            if (_currentTab != null)
            {
                _currentTab.Button.SetSelected(false);
                SetContentActive(_currentTab, false);
            }

            _currentTab = entry;
            _currentTab.Button.SetSelected(true);
            SetContentActive(_currentTab, true);
        }

        private static void SetContentActive(TabEntry entry, bool active)
        {
            foreach (var obj in entry.Content)
                if (obj != null)
                    obj.SetActive(active);
        }

        [Button]
        private void SelectDefault()
        {
            if (_tabs.Count > 0)
                SelectTab(_defaultTabIndex);
        }
    }
}
