using System.Linq;
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
        public readonly Rect? Position;
        public readonly string Title;
        public readonly Object menuCommandContext;

        public CreateGameObjectPlacematCommand()
        {
            UndoString = "Create Placemat";
        }
        
        public CreateGameObjectPlacematCommand(Rect? position, string title = null, Object menuCommandContext = null) : this()
        {
            Position = position;
            Title = title;
            this.menuCommandContext = menuCommandContext;
        }
        
        public static async void DefaultCommandHandler(IState state, CreateGameObjectPlacematCommand command)
        {
            var graphToolState = (ScriptGraphState)state;
            graphToolState.PushUndo(command);
            var assetModel = (ScriptGraphAsset)graphToolState.GraphViewState.AssetModel;

            var context = command.menuCommandContext;
            
            GameObject selectedGameObject = (GameObject)(context ? context : 
                await GameObjectSelectorWindow.ShowWindowAsync());

            ReferencesDatabase referencesDatabase;
            GameObject relatedPrefab = null;

            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if (PrefabUtility.IsPartOfAnyPrefab(selectedGameObject) || 
                prefabStage != null)
            {
                
                relatedPrefab = PrefabUtility.GetNearestPrefabInstanceRoot(selectedGameObject) ?? prefabStage.prefabContentsRoot;
                if ((referencesDatabase = relatedPrefab.GetComponent<ReferencesDatabase>()) == null)
                {
                    referencesDatabase = relatedPrefab.AddComponent<ReferencesDatabase>();
                }

            }
            else
            {
                referencesDatabase = RuntimeUtility.FindRelatedRuntimeOnActiveScene(assetModel).ReferencesDatabase;
            }
            

            using (var graphUpdater = graphToolState.GraphViewState.UpdateScope)
            {
                var guid = GetGuidForObject(selectedGameObject, referencesDatabase);
                
                var scriptGraphModel = (ScriptGraphModel)graphToolState.GraphViewState.GraphModel;
                var positionToSpawn = command.Position ?? new Rect(scriptGraphModel.NodeModels.First().Position, Vector2.zero);
                var placematModel = scriptGraphModel.CreateGameObjectPlacemat(positionToSpawn, guid, selectedGameObject.name) as GameObjectPlacematModel;
                placematModel.RelatedPrefab = relatedPrefab;
                
                if (command.Title != null)
                    placematModel.Title = command.Title;

                graphUpdater.MarkNew(placematModel);
            }
        }

        private static SerializableGUID GetGuidForObject(GameObject gameObject, ReferencesDatabase runtime)
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