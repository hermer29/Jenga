using System;
using System.Linq;
using System.Reflection;
using Jenga.Core.Utilities;
using Jenga.Editor.Base;
using Jenga.Editor.Features.GameObjectGraphPresentation;
using Jenga.Editor.Features.ScriptsSerialization;
using Jenga.Editor.Utility;
using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;
using UnityEngine;
using UnityEngine.GraphToolsFoundation.Overdrive;

namespace Jenga.Editor.ScriptNode
{
    [Serializable]
    [SearcherItem(typeof(ScriptGraphStencil), SearcherContext.Graph, "")]
    public class ScriptNodeModel : NodeModel, IGameObjectPlacematSuitableNode
    {
        public string MonoScriptGuid;
        [SerializeField] public ScriptSerializer Serializer;
        public SerializableGUID ContainingGameObjectPlacemat;

        public SerializableGUID ContainingPlacematGuid
        {
            get => ContainingGameObjectPlacemat; 
            set => ContainingGameObjectPlacemat = value;
        }
        
        public ScriptNodeModel()
        {
            Title = "Context Horizontal";
        }

        public Type MonoScriptType => AssetUtility.GetMonoScriptType(MonoScriptGuid);

        public void Initialize(string scriptGuid)
        {
            if (scriptGuid == null)
                return;
            MonoScriptGuid = scriptGuid;
            Title = MonoScriptType.Name;
        }

        public override void OnDestroyed()
        {
            Serializer.Dispose();
        }

        protected override void OnDefineNode()
        {
            base.OnDefineNode();

            if (string.IsNullOrEmpty(MonoScriptGuid))
                return;

            Serializer = new ScriptSerializer((ScriptGraphModel)m_AssetModel.GraphModel,
                Guid.ToGUID(), MonoScriptGuid, RuntimeUtility.FindRelatedRuntimeOnActiveScene((GraphAssetModel)AssetModel));
            var type = MonoScriptType;
            Title = type.Name;

            foreach (var methodInfo in ReflectionUtility.GetPublicMethods(type))
            { 
                var inputPort = this.AddDataInputPort(methodInfo.Name, TypeHandle.Float);
            }
            
            foreach (var publicInstanceEvent in ReflectionUtility.GetPublicInstanceEvents(type))
            {
                this.AddDataOutputPort(publicInstanceEvent.Name, TypeHandle.Float);
            }
        }

        public override void OnDragEnded()
        {
            foreach (var graphModelPlacematModel in GraphModel.PlacematModels)
            {
                if (graphModelPlacematModel is GameObjectPlacematModel placematModel)
                {
                    if (RectUtils.IntersectsSegment(placematModel.PositionAndSize, Position, Position))
                    {
                        placematModel.ContainingNodes.Add(Guid);
                        ContainingGameObjectPlacemat = placematModel.Guid;
                    }
                    else
                    {
                        if (placematModel.ContainingNodes.Contains(Guid))
                        {
                            ContainingGameObjectPlacemat = default;
                            placematModel.ContainingNodes.Remove(Guid);
                        }
                    }
                }
            }
        }

    }
}
