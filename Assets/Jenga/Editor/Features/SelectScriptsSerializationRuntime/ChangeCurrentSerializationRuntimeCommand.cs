using System.Linq;
using Jenga.Runtime;
using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts;
using UnityEngine.GraphToolsFoundation.CommandStateObserver;

namespace Jenga.Editor.DataModel.Commands
{
    public class ChangeCurrentSerializationRuntimeCommand : UndoableCommand
    {

        public JengaRuntime SelectedRuntime;
        
        public static void DefaultCommandHandler(IState graphToolState, ChangeCurrentSerializationRuntimeCommand command)
        {
            if (command.SelectedRuntime == null)
                return;

            (graphToolState as GraphToolState).PushUndo(command);
            var state = graphToolState.AllStateComponents.First(x => x is CurrentScriptSerializationRuntimeState) as CurrentScriptSerializationRuntimeState;
            state.UpdateScope.ChangeJengaRuntime(command.SelectedRuntime);
        }
    }
}