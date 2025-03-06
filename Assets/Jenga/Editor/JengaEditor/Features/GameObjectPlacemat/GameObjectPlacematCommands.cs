using System.Linq;
using Jenga.Core;
using Jenga.Editor.Base;
using Jenga.Editor.ScriptNode;
using Jenga.Editor.Utility;
using Jenga.Runtime;
using UnityEditor;
using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;
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

            var referencesDatabase = GetRelatedReferencesDatabase(ref selectedGameObject, assetModel, out var relatedPrefab, out var prefabPath);

            using (var graphUpdater = graphToolState.GraphViewState.UpdateScope)
            {
                var guid = GetGuidForObject(selectedGameObject, referencesDatabase, relatedPrefab, prefabPath);
                
                var scriptGraphModel = (ScriptGraphModel)graphToolState.GraphViewState.GraphModel;
                var positionToSpawn = command.Position ?? new Rect(scriptGraphModel.NodeModels.First().Position, Vector2.zero);
                var placematModel = scriptGraphModel.CreateGameObjectPlacemat(positionToSpawn, guid, selectedGameObject.name) as GameObjectPlacematModel;
                placematModel.RelatedPrefab = relatedPrefab;
                
                if (command.Title != null)
                    placematModel.Title = command.Title;

                graphUpdater.MarkNew(placematModel);
            }
        }

        private static ReferencesDatabase GetRelatedReferencesDatabase(ref GameObject selectedGameObject,
            ScriptGraphAsset assetModel, out GameObject relatedPrefab, out string prefabPath)
        {
            ReferencesDatabase referencesDatabase;
            relatedPrefab = null;
            prefabPath = string.Empty;
            
            if (PrefabUtility.IsPartOfAnyPrefab(selectedGameObject)) // In Prefab Instance
            {
                relatedPrefab = PrefabUtility.GetNearestPrefabInstanceRoot(
                    PrefabUtility.GetPrefabInstanceHandle(selectedGameObject));
                prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(selectedGameObject);
                referencesDatabase = GetReferencesDatabaseFromPrefabRoot(relatedPrefab);

                Assert.IsNotNull(relatedPrefab, "relatedPrefab != null");
                
            }
            else if (PrefabStageUtility.GetCurrentPrefabStage() != null) // In Prefab Stage 
            {
                var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                relatedPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabStage.assetPath);
                prefabPath = prefabStage.assetPath;
                var prefabStageSelectedObjectPath = HierarchyUtility.GetGameObjectPathWithIndex(selectedGameObject);
                selectedGameObject = HierarchyUtility.FindObjectByPath(prefabStageSelectedObjectPath, relatedPrefab);

                referencesDatabase = GetReferencesDatabaseFromPrefabRoot(relatedPrefab);
            }
            else // In Scene 
            {
                referencesDatabase = RuntimeUtility.FindRelatedRuntimeOnActiveScene(assetModel).ReferencesDatabase;
            }

            Assert.IsNotNull(referencesDatabase, "referencesDatabase != null");
            return referencesDatabase;
        }

        private static ReferencesDatabase GetReferencesDatabaseFromPrefabRoot(GameObject relatedPrefab)
        {
            ReferencesDatabase referencesDatabase;
            if ((referencesDatabase = relatedPrefab.GetComponent<ReferencesDatabase>()) == null)
            {
                referencesDatabase = relatedPrefab.AddComponent<ReferencesDatabase>();
            }

            return referencesDatabase;
        }

        private static SerializableGUID GetGuidForObject(GameObject gameObject, ReferencesDatabase runtime, GameObject prefabRoot,
            string prefabPath)
        {
            var existedGuidForThisObject = runtime.GetGuidByObjectReference(gameObject);
            if (existedGuidForThisObject != default)
            {
                return existedGuidForThisObject.ToVanillaSerializableGUID();
            }

            var newGuid = SerializableGUIDJengaVersion.Generate();
            runtime.AddObjectReference(newGuid, gameObject);
            return newGuid.ToVanillaSerializableGUID();
        }
    }
}