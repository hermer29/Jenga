using System;
using System.Collections.Generic;
using System.Linq;
using Jenga.Core.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;
using UnityEngine.GraphToolsFoundation.Overdrive;

namespace Jenga.Editor.Features.GameObjectGraphPresentation
{
    [Serializable]
    public class GameObjectPlacematModel : PlacematModel
    {
        public SerializableGUID GameObjectGuid;
        public SerializedHashset<SerializableGUID> ContainingNodes = new SerializedHashset<SerializableGUID>();

        public override void OnDragEnded()
        {
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