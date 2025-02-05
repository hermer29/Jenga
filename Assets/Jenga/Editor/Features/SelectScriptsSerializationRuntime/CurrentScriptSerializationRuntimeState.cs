using System;
using Jenga.Runtime;
using UnityEngine;
using UnityEngine.GraphToolsFoundation.CommandStateObserver;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts
{
    [Serializable]
    public class CurrentScriptSerializationRuntimeState : StateComponent<CurrentScriptSerializationRuntimeState.StateUpdater>
    {
        [SerializeField]
        public JengaRuntime CurrentJengaRuntime;

        public class StateUpdater : BaseUpdater<CurrentScriptSerializationRuntimeState>
        {
            public void ChangeJengaRuntime(JengaRuntime runtime)
            {
                m_State.CurrentJengaRuntime = runtime;
                m_State.SetUpdateType(UpdateType.Complete);
            }
        }

        protected override void Dispose(bool disposing)
        {
            
        }
    }
}