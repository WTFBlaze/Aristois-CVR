using ABI.CCK.Components;
using ABI_RC.Core.InteractionSystem;
using ABI_RC.Core.Player;
using Aristois.Components;
using Aristois.Configs;
using Aristois.Modules.Core;
using Aristois.Utils;
using BTKUILib.UIObjects.Components;
using UnityEngine;

namespace Aristois.Modules.Visual
{
    internal class CapsuleEsp : ModuleBase
    {
        protected override bool HideFromList => false;
        protected override string ModuleName => "Capsule Esp";
        private ToggleButton ToggleButton { get; set; }

        public override void OnAvatarInstantiated(PuppetMaster puppetMaster)
        {
            if (puppetMaster.name == "_PLAYERLOCAL")
                return;

            var yValue = puppetMaster.gameObject.GetComponentInChildren<CVRAvatar>().viewPosition.y;
            var espTransform = puppetMaster.transform.Find("Esp");
            if (espTransform != null)
            {
                espTransform.localPosition = new Vector3(0, yValue / 1.6f, 0);
                espTransform.localScale = new Vector3(yValue / 1.6f, yValue / 1.4f, yValue / 1.5f);
                espTransform.gameObject.SetActive(State);
                return;
            }
            puppetMaster.gameObject.AddComponent<EspOutline>();
            espTransform.gameObject.SetActive(State);
        }

        public override void OnUILoaded(ref CVR_MenuManager menuManager)
        {
            ToggleButton = UserInterface.Category_Esp.AddToggle("Capsule Esp", "Show a capsule shaped bubble around all players in the instance.", State);
            ToggleButton.OnValueUpdated += (value) =>
            {
                Config.Main.CapsuleEsp = value;
                SetState(value);
            };

            var rainbowTgl = UserInterface.Category_Esp_Settings.AddToggle("Rainbow Fade", "Sets all Esp Modules to use a Rainbow Fade effect", Config.Colors.RainbowEsp);
            rainbowTgl.OnValueUpdated += (value) =>
            {
                Config.Colors.RainbowEsp = value;
                UpdateAllEspItems();
            };

            var gradientTgl = UserInterface.Category_Esp_Settings.AddToggle("Gradient Colors", "Sets all Esp Modules to use a two color Gradient effect", Config.Colors.GradientEsp);
            gradientTgl.OnValueUpdated += (value) =>
            {
                Config.Colors.GradientEsp = value;
                UpdateAllEspItems();
            };

            var scanLineTgl = UserInterface.Category_Esp_Settings.AddToggle("ScanLine Effect", "Sets all Esp Modules to use a scan line effect (sets to color of universal esp color in config)", Config.Colors.ScanLinesEsp);
            scanLineTgl.OnValueUpdated += (value) =>
            {
                Config.Colors.ScanLinesEsp = value;
                UpdateAllEspItems();
            };
        }

        private void UpdateAllEspItems()
        {
            foreach (var player in CVRUtils.GetAllPlayers())
            {
                var espTransform = player.PuppetMaster.transform.Find("Esp");
                if (espTransform is null)
                    continue;
                espTransform.gameObject.SetActive(State);
                var espComp = player.PuppetMaster.GetComponent<EspOutline>();
                if (espComp is null)
                    continue;

                if (Config.Colors.RainbowEsp)
                    espComp.SetRainbowEsp();
                else if (Config.Colors.GradientEsp)
                    espComp.SetGradientEsp();
                else if (Config.Colors.ScanLinesEsp)
                    espComp.SetScanLinesEsp();
                else
                    espComp.SetSingleColorEsp();
            }
        }

        protected override void OnStateChange(bool state)
        {
            if (state && !BlacklistManager.CanUseRiskyFunctions)
            {
                SetState(false);
                return;
            }

            UpdateAllEspItems();
            if (ToggleButton != null && ToggleButton.ToggleValue != state)
                ToggleButton.ToggleValue = state;
        }

        public override void OnRiskyDetermined(bool canUseFunctions)
        {
            if (canUseFunctions || !State)
                return;
            SetState(false);
        }
    }
}
