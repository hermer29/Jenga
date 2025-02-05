using System;
using System.Collections.Generic;
using Jenga.Editor.DataModel.Commands;
using UnityEngine;
using UnityEngine.GraphToolsFoundation.CommandStateObserver;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts
{
    [Serializable]
    public class ScriptGraphState : GraphToolState
    {

        [SerializeField]
        CurrentScriptSerializationRuntimeState currentScriptSerializationRuntimeState;
        
        /// <inheritdoc />
        public ScriptGraphState(Hash128 graphViewEditorWindowGUID, Preferences preferences)
            : base(graphViewEditorWindowGUID, preferences)
        {
            this.SetInitialSearcherSize(SearcherService.Usage.k_CreateNode, new Vector2(500, 400), 2.25f);
        }

        public IStateComponent CurrentSerializationRuntimeState => 
            currentScriptSerializationRuntimeState ??= PersistedState.GetOrCreateViewStateComponent<CurrentScriptSerializationRuntimeState>(m_GraphViewEditorWindowGUID, nameof(CurrentSerializationRuntimeState));

        public override IEnumerable<IStateComponent> AllStateComponents
        {
            get
            {
                foreach (var allStateComponent in base.AllStateComponents)
                {
                    yield return allStateComponent;
                }

                yield return CurrentSerializationRuntimeState;
            }
        }

        public override void RegisterCommandHandlers(Dispatcher dispatcher)
        {
            base.RegisterCommandHandlers(dispatcher);
            dispatcher.RegisterCommandHandler<ChangeCurrentSerializationRuntimeCommand>(ChangeCurrentSerializationRuntimeCommand.DefaultCommandHandler);
        }
    }
}
