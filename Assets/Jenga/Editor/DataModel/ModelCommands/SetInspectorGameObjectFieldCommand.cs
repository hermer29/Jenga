using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.GraphToolsFoundation.CommandStateObserver;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts.ModelCommands
{
    public class SetInspectorGameObjectFieldCommand : ModelCommand<ScriptNodeModel, GameObject[]>
    {
        const string k_UndoStringSingular = "Set GameObject Field";
        const string k_UndoStringPlural = "Set GameObject Fields";
        
        public SetInspectorGameObjectFieldCommand(ScriptNodeModel[] nodes, GameObject[] value) 
            : base(k_UndoStringSingular, k_UndoStringPlural, value, nodes.ToList().AsReadOnly())
        {
        }

        public static void DefaultHandler(IState state, SetInspectorGameObjectFieldCommand command)
        {
            foreach (var scriptNodeModel in command.Models)
            {
                scriptNodeModel.GameObjects = command.Value;
            }
        }
    }
}