using ABI_RC.Core.InteractionSystem;
using Aristois.Core;
using Aristois.Utils;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Aristois.Modules.Visual
{
    internal class DebugPanel : ModuleBase
    {
        protected override bool HideFromList => false;
        protected override string ModuleName => "Debug Panel";

        private static GameObject _panelObj;
        private static TextMeshProUGUI _templateComp;
        private static readonly List<string> _LogsForAfterCreation = new();

        public override void OnUILoaded(ref CVR_MenuManager menuManager)
        {
            try
            {
                Transform menuTransform = menuManager.transform.Find("QuickMenu");
                _panelObj = Object.Instantiate(BundleManager.PanelPrefab, menuTransform);
                _panelObj.SetActive(false);
                _panelObj.name = "VRTool Debugger";
                var rt = _panelObj.GetComponent<RectTransform>();
                rt.localScale = new Vector3(0.001f, 0.001f, 1);
                rt.anchoredPosition = new Vector2(-0.63f, 0.05f);
                _templateComp = _panelObj.transform.Find("Background/Container/Scroll View/Viewport/Content/Template").GetComponent<TextMeshProUGUI>();
                _templateComp.gameObject.SetActive(false);
                _templateComp.text = string.Empty;
                _templateComp.richText = true;

                AddLog(string.Format("<color={0}>Aristois</color> Debugger Started! <color=red><3</color>", ColorManager.Hex.Aqua));
                WritePreLoadedLogs();
            }
            catch (System.Exception ex)
            {
                Logs.Error(ex);
            }
        }

        public override void OnUIToggled(bool newActiveState)
            => _panelObj?.SetActive(newActiveState);

        internal static void AddLog(string msg)
        {
            if (_templateComp is null)
                _LogsForAfterCreation.Add(msg);
            else
                CreateLogObject(msg);
        }

        internal static void AddLog(string prefix, string hexColor, string msg)
        {
            var fullMsg = string.Format("<color={0}>[{1}]</color> {2}", hexColor, prefix, msg);

            if (_templateComp is null)
                _LogsForAfterCreation.Add(fullMsg);
            else
                CreateLogObject(fullMsg);
        }

        internal static void ClearDebug()
        {
            var parentTransform = _templateComp.transform.parent;
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                Transform child = parentTransform.GetChild(i);
                if (child is null || child == _templateComp.transform)
                    continue;
                Object.Destroy(child.gameObject);
            }
        }

        private static void CreateLogObject(string msg)
        {
            var fullMsg = string.Format("<i><color=green>({0})</color></i> {1}", System.DateTime.Now.ToString("HH:mm:ss"), msg);
            var obj = Object.Instantiate(_templateComp.gameObject, _templateComp.transform.parent);
            obj.name = "Log Item";
            obj.SetActive(true);
            var comp = obj.GetComponent<TextMeshProUGUI>();
            comp.text = fullMsg;
        }

        private void WritePreLoadedLogs()
        {
            foreach (var log in _LogsForAfterCreation)
                AddLog(log);
        }
    }
}
