using System;
using Jenga.Editor.Features.EditScriptContextMenu;
using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEngine;
using UnityEngine.GraphToolsFoundation.CommandStateObserver;

namespace Jenga.Editor.Base
{
    [Serializable]
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
            dispatcher.RegisterCommandHandler<EditScriptCommand>(EditScriptCommand.DefaultCommandHandler);
        }
    }
}
