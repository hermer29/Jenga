using Jenga.Editor.Base;
using Jenga.Editor.ScriptNode;
using Jenga.Editor.Utility;
using Jenga.Runtime;
using UnityEditor;
using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEngine;
using UnityEngine.GraphToolsFoundation.CommandStateObserver;
using UnityEngine.GraphToolsFoundation.Overdrive;

namespace Jenga.Editor.Features.GameObjectGraphPresentation
{
    
    
    public class CreateGameObjectPlacematCommand : UndoableCommand
    {
        public Rect Position;
        public string Title;

        public CreateGameObjectPlacematCommand()
        {
            UndoString = "Create Placemat";
        }
        
        public CreateGameObjectPlacematCommand(Rect position, string title = null) : this()
        {
            Position = position;
            Title = title;
        }
        
        public static async void DefaultCommandHandler(IState state, CreateGameObjectPlacematCommand command)
        {
            var graphToolState = state as ScriptGraphState;
            graphToolState.PushUndo(command);
            var assetModel = (GraphAssetModel)graphToolState.GraphViewState.AssetModel;
            var runtime = RuntimeUtility.FindRelatedRuntimeOnActiveScene(assetModel);
            
            if (runtime == null)
            {
                Debug.LogError("Runtime with this graph asset on active scene is not found!");
                return;
            }
            
            var selectedGameObject = await GameObjectSelectorWindow.ShowWindowAsync();

            using (var graphUpdater = graphToolState.GraphViewState.UpdateScope)
            {
                var guid = GetGuidForObject(selectedGameObject, runtime);
                
                var scriptGraphModel = graphToolState.GraphViewState.GraphModel as ScriptGraphModel;
                var placematModel = scriptGraphModel.CreateGameObjectPlacemat(command.Position, guid, selectedGameObject.name);

                if (command.Title != null)
                    placematModel.Title = command.Title;

                graphUpdater.MarkNew(placematModel);
            }
        }

        private static SerializableGUID GetGuidForObject(GameObject gameObject, JengaRuntime runtime)
        {
            var existedGuidForThisObject = runtime.GetGuidByObjectReference(gameObject);
            if (existedGuidForThisObject != null)
            {
                return existedGuidForThisObject.Value.ToSerializableGUID();
            }

            var newGuid = SerializableGUID.Generate();
            runtime.AddObjectReference(newGuid.ToGUID(), gameObject);
            return newGuid;
        }
    }
}