using System;
using System.Collections.Generic;
using UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts.ModelCommands;
using UnityEngine;
using UnityEngine.GraphToolsFoundation.CommandStateObserver;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts
{
    public class ScriptGraphState : GraphToolState
    {
        
        /// <inheritdoc />
        public ScriptGraphState(Hash128 graphViewEditorWindowGUID, Preferences preferences)
            : base(graphViewEditorWindowGUID, preferences)
        {
            this.SetInitialSearcherSize(SearcherService.Usage.k_CreateNode, new Vector2(500, 400), 2.25f);
        }

        public override void RegisterCommandHandlers(Dispatcher dispatcher)
        {
            base.RegisterCommandHandlers(dispatcher);
            
            dispatcher.RegisterCommandHandler<SetInspectorGameObjectFieldCommand>(SetInspectorGameObjectFieldCommand.DefaultHandler);
        }
    }
}
