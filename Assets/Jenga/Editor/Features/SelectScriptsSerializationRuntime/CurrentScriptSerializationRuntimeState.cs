using System;
using Jenga.Runtime;
using UnityEngine;
using UnityEngine.GraphToolsFoundation.CommandStateObserver;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts
{
    [Serializable]
    public class CurrentScriptSerializationRuntimeState : StateComponent<CurrentScriptSerializationRuntimeState.StateUpdater>, IViewStateComponent
    {
        [SerializeField]
        public JengaRuntime CurrentJengaRuntime;

        public class StateUpdater : BaseUpdater<CurrentScriptSerializationRuntimeState>
        {
            public void ChangeJengaRuntime(JengaRuntime runtime)
            {
                m_State.CurrentJengaRuntime = runtime;
                m_State.SetUpdateType(UpdateType.Complete);
                Debug.Log($"Now current jenga runtime is: {m_State.CurrentJengaRuntime.name}", m_State.CurrentJengaRuntime.transform);
            }
        }

        protected override void Dispose(bool disposing)
        {
            
        }

        public Hash128 ViewGUID { get; set; }
    }
}