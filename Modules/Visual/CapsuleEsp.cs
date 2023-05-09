using ABI.CCK.Components;
using ABI_RC.Core.Player;
using Aristois.Components;
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

        public override void OnUILoaded()
        {
            ToggleButton = UserInterface.Category_Esp.AddToggle("Capsule Esp", "Show a capsule shaped bubble around all players in the instance.", State);
            ToggleButton.OnValueUpdated += (value) =>
            {
                SetState(value);
            };
        }

        protected override void OnStateChange(bool state)
        {
            foreach (var player in CVRUtils.GetAllPlayers())
            {
                var espTransform = player.PuppetMaster.transform.Find("Esp");
                if (espTransform is null)
                    continue;
                espTransform.gameObject.SetActive(state);
            }
            if (ToggleButton != null && ToggleButton.ToggleValue != state)
                ToggleButton.ToggleValue = state;
        }
    }
}
