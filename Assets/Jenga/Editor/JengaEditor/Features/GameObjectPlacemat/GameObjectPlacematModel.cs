using System;
using System.Linq;
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
            ContainingNodes.OnElementAdded += OnElementAdded;
            ContainingNodes.OnElementRemoved += OnElementRemoved;
            
            void OnElementAdded(SerializableGUID scriptNodesGuid)
            {
                var scriptGraphModel = GraphModel as ScriptGraphModel;
                var referencesDatabase = GetRelatedReferencesDatabase();
                var assetModel = (ScriptGraphAsset) scriptGraphModel.AssetModel;
                var gameObjectGuid = GameObjectGuid.Value;
                var gameObject = (GameObject) referencesDatabase.GetObjectReferenceByGuid(gameObjectGuid.ToGUID());
                var componentGuid = scriptNodesGuid;
                var scriptNodeModel = (ScriptNodeModel)assetModel.GraphModel.NodeModels.FirstOrDefault(x =>
                    x is ScriptNodeModel nodeModel && nodeModel.PersistentGuid == componentGuid);
                referencesDatabase.AddObjectReference(componentGuid.ToGUID(), gameObject.AddComponent(scriptNodeModel.MonoScriptType));
            }

            void OnElementRemoved(SerializableGUID scriptNodeGuid)
            {
                var referencesDatabase = GetRelatedReferencesDatabase();
                var component = referencesDatabase.GetObjectReferenceByGuid(scriptNodeGuid.ToGUID());
                Object.DestroyImmediate(component);
                referencesDatabase.RemoveObjectReference(scriptNodeGuid.ToGUID());
            }
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