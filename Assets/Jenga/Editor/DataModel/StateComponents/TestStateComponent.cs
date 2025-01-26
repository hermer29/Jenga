using UnityEngine.GraphToolsFoundation.CommandStateObserver;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts
{
    public class TestStateComponent : StateComponent<TestStateUpdater>
    {
        protected override void Dispose(bool disposing)
        {
            
        }
    }
    
    public class TestStateUpdater : IStateComponentUpdater
    {
        public void Dispose()
        {
            
        }

        public void Initialize(IStateComponent state)
        {
            
        }
    }
}