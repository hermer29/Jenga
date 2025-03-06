using System;
using System.Linq;
using Jenga.Core;
using Jenga.Core.Utilities;
using Jenga.Core.Utilities.Reactivity;
using Jenga.Editor.Base;
using Jenga.Editor.ScriptNode;
using Jenga.Editor.Utility;
using Jenga.Runtime;
using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;
using UnityEngine;
using UnityEngine.GraphToolsFoundation.Overdrive;
using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;
using RectUtils = UnityEditor.GraphToolsFoundation.Overdrive.RectUtils;

namespace Jenga.Editor.Features.GameObjectGraphPresentation
{
    [Serializable]
    public sealed class GameObjectPlacematModel : PlacematModel
    {
        public ReactiveProperty<SerializableGUID> GameObjectGuid = new ReactiveProperty<SerializableGUID>(default);
        public SerializedReactiveHashset<SerializableGUID> ContainingNodes = new SerializedReactiveHashset<SerializableGUID>();
        public GameObject RelatedPrefab;

        public GameObjectPlacematModel() : base()
        {
            ContainingNodes.OnElementAdded += OnNodeEnteredPlacemat;
            ContainingNodes.OnElementRemoved += OnNodeLeftPlacemat;
        }

        private void OnNodeEnteredPlacemat(SerializableGUID scriptNodesGuid)
        {
            var scriptGraphModel = GraphModel as ScriptGraphModel;
            var referencesDatabase = GetRelatedReferencesDatabase();
            Assert.IsNotNull(referencesDatabase, "referencesDatabase != null");
            var assetModel = (ScriptGraphAsset) scriptGraphModel.AssetModel;
            var gameObjectGuid = GameObjectGuid.Value;
            var gameObject = referencesDatabase.GetObjectReferenceByGuid<GameObject>(gameObjectGuid.ToSerializableGUIDJengaVersion());
            Assert.IsNotNull(gameObject, $"gameObject != null, reference path: {HierarchyUtility.GetGameObjectPathWithIndex(referencesDatabase.gameObject)}");
            var componentGuid = scriptNodesGuid;
            var scriptNodeModel = (ScriptNodeModel)assetModel.GraphModel.NodeModels.FirstOrDefault(x =>
                x is ScriptNodeModel nodeModel && nodeModel.PersistentGuid == componentGuid);
            Assert.IsNotNull(scriptNodeModel, "scriptNodeModel != null");
            referencesDatabase.AddObjectReference(componentGuid.ToSerializableGUIDJengaVersion(), 
                gameObject.AddComponent(scriptNodeModel.MonoScriptType));
        }

        private void OnNodeLeftPlacemat(SerializableGUID scriptNodeGuid)
        {
            var referencesDatabase = GetRelatedReferencesDatabase();
            var component = referencesDatabase.GetObjectReferenceByGuid<Object>(scriptNodeGuid.ToSerializableGUIDJengaVersion());
            Object.DestroyImmediate(component);
            referencesDatabase.RemoveObjectReference(scriptNodeGuid.ToSerializableGUIDJengaVersion());
        }

        private ReferencesDatabase GetRelatedReferencesDatabase()
        {
            if (RelatedPrefab != null)
            {
                return RelatedPrefab.GetComponent<ReferencesDatabase>();
            }
            var scriptGraphModel = GraphModel as ScriptGraphModel;
            return RuntimeUtility.FindRelatedRuntimeOnActiveScene((GraphAssetModel)scriptGraphModel.AssetModel).ReferencesDatabase;
        }

        public override void OnDragEnded()
        {
            // TODO: ЭТО ДОЛЖНО БЫТЬ В ДРУГОМ МЕСТЕ? 
            foreach (var scriptNodeModel in GraphModel.NodeModels.OfType<IGameObjectPlacematSuitableNode>())
            {
                if (scriptNodeModel is ScriptNodeModel nodeModel)
                {
                    if (RectUtils.IntersectsSegment(PositionAndSize, nodeModel.Position, nodeModel.Position))
                    {
                        (nodeModel as IGameObjectPlacematSuitableNode).ContainingPlacematGuid = Guid;
                        ContainingNodes.Add(nodeModel.PersistentGuid);
                    }
                    else
                    {
                        if (ContainingNodes.Contains(nodeModel.Guid))
                        { 
                            (nodeModel as IGameObjectPlacematSuitableNode).ContainingPlacematGuid = default;
                            ContainingNodes.Remove(nodeModel.PersistentGuid);
                        }
                    }
                }
            }
        }
    }
}