using System;
using System.Linq;
using Jenga.Core.Utilities;
using Jenga.Core.Utilities.Reactivity;
using Jenga.Editor.Base;
using Jenga.Editor.ScriptNode;
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

        public GameObjectPlacematModel() : base()
        {
            ContainingNodes.OnElementAdded += OnElementAdded;
            ContainingNodes.OnElementRemoved += OnElementRemoved;
            
            void OnElementAdded(SerializableGUID scriptNodesGuid)
            {
                var scriptGraphModel = GraphModel as ScriptGraphModel;
                var runtime = scriptGraphModel.CurrentRuntime.Value;
                var assetModel = scriptGraphModel.AssetModel;
                var gameObjectGuid = GameObjectGuid.Value;
                var gameObject = (GameObject) runtime.GetObjectReferenceByGuid(gameObjectGuid.ToGUID());
                var componentGuid = scriptNodesGuid;
                assetModel.GraphModel.TryGetModelFromGuid(scriptNodesGuid, out var graphNode);
                var scriptNodeModel = (ScriptNodeModel)graphNode;
                runtime.AddObjectReference(componentGuid.ToGUID(), gameObject.AddComponent(scriptNodeModel.MonoScriptType));
            }

            void OnElementRemoved(SerializableGUID scriptNodeGuid)
            {
                var scriptGraphModel = GraphModel as ScriptGraphModel;
                var runtime = scriptGraphModel.CurrentRuntime.Value;
                var component = runtime.GetObjectReferenceByGuid(scriptNodeGuid.ToGUID());
                Object.DestroyImmediate(component);
                runtime.RemoveObjectReference(scriptNodeGuid.ToGUID());
            }
        }

        public override void OnDragEnded()
        {
            // TODO: ЭТО ДОЛЖНО БЫТЬ В ДРУГОМ МЕСТЕ? 
            foreach (var graphModelPlacematModel in GraphModel.NodeModels.OfType<IGameObjectPlacematSuitableNode>())
            {
                if (graphModelPlacematModel is NodeModel nodeModel)
                {
                    if (RectUtils.IntersectsSegment(PositionAndSize, nodeModel.Position, nodeModel.Position))
                    {
                        (nodeModel as IGameObjectPlacematSuitableNode).ContainingPlacematGuid = Guid;
                        ContainingNodes.Add(nodeModel.Guid);
                    }
                    else
                    {
                        if (ContainingNodes.Contains(nodeModel.Guid))
                        { 
                            (nodeModel as IGameObjectPlacematSuitableNode).ContainingPlacematGuid = default;
                            ContainingNodes.Remove(nodeModel.Guid);
                        }
                    }
                }
            }
        }
    }
}