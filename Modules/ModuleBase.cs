using ABI_RC.Core.InteractionSystem;
using ABI_RC.Core.Networking.IO.Instancing;
using ABI_RC.Core.Player;

namespace Aristois.Modules
{
    internal class ModuleBase
    {
        #region Core Module Fields & Methods
        protected virtual string ModuleName => "Unnamed Module";
        protected virtual bool HideFromList => true;
        public bool State { get; private set; }
        protected void SetState(bool? newState = null)
        {
            if ((newState ?? !State) == State)
                return;
            State = newState ?? !State;
            OnStateChange(State);
        }
        public string GetModuleName() => ModuleName;
        public bool GetHideFromList() => HideFromList;
        #endregion

        #region Module Override Methods
        public virtual void OnStart() { }
        public virtual void OnStop() { }
        public virtual void OnUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnConfigsLoaded() { }
        protected virtual void OnStateChange(bool state) { }
        public virtual void OnSceneLoaded(int index, string name) { }
        public virtual void OnSceneUnloaded(int index, string name) { }
        public virtual void OnUILoaded(ref CVR_MenuManager menuManager) { }
        public virtual void OnUIToggled(bool newActiveState) { }
        public virtual void OnContainerCreated() { }
        public virtual void OnKeybindCalled() { }
        public virtual void OnAvatarInstantiated(PuppetMaster puppetMaster) { }
        public virtual void OnPlayerJoined(CVRPlayerEntity player) { }
        public virtual void OnPlayerLeft(CVRPlayerEntity player) { }
        public virtual void OnWorldLoaded() { }
        public virtual void OnRiskyDetermined(bool canUseFunctions) { }
        #endregion
    }
}
