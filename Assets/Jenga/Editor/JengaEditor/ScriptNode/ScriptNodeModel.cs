using System;
using System.Linq;
using Jenga.Core.Utilities;
using Jenga.Editor.Base;
using Jenga.Editor.Features.GameObjectGraphPresentation;
using Jenga.Editor.Features.ScriptsSerialization;
using Jenga.Editor.Utility;
using Jenga.Runtime;
using UnityEditor;
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
        [SerializeReference] public GameObjectPlacematModel RelatedGameOjbectPlacemat;
        public SerializableGUID PersistentGuid;

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
            PersistentGuid = SerializableGUID.Generate();
            MonoScriptGuid = scriptGuid;
            Title = MonoScriptType.Name;
        }

        public override void OnDestroyed()
        {
            
        }

        public override void OnConnection(IPortModel selfConnectedPortModel, IPortModel otherConnectedPortModel)
        {
            var isThisNodeAReceiver = selfConnectedPortModel.Direction == PortDirection.Input;
            var receiver = (ScriptNodeModel) (isThisNodeAReceiver ? this : otherConnectedPortModel.NodeModel);
            var sender = (ScriptNodeModel) (!isThisNodeAReceiver ? this : otherConnectedPortModel.NodeModel);
            var receiversPort = isThisNodeAReceiver ? selfConnectedPortModel : otherConnectedPortModel;
            var sendersPort = !isThisNodeAReceiver ? selfConnectedPortModel : otherConnectedPortModel;
            var receiversPlacemat = GraphModel.PlacematModels.OfType<GameObjectPlacematModel>()
                .FirstOrDefault(x => x.Guid == receiver.ContainingPlacematGuid);
            var sendersPlacemat = GraphModel.PlacematModels.OfType<GameObjectPlacematModel>()
                .FirstOrDefault(x => x.Guid == sender.ContainingPlacematGuid);

            if (sendersPlacemat == null || receiversPlacemat == null)
                return;

            var eventData = new NodeEvent
            {
                ReceiversGuid = receiver.PersistentGuid.ToSerializableGUIDJengaVersion(),
                SendersGuid = sender.PersistentGuid.ToSerializableGUIDJengaVersion(),
                ReceiversMethodName = receiversPort.UniqueName,
                SendersEventName = sendersPort.UniqueName,
                
                ReceiversPrefab = receiversPlacemat.RelatedPrefab,
                SendersPrefab = sendersPlacemat.RelatedPrefab
            };

            if (receiversPlacemat.RelatedPrefab != null)
            {
                var database = receiversPlacemat.RelatedPrefab.GetComponent<ReferencesDatabase>();
                database.AddEvent(eventData);
            }
            if (sendersPlacemat.RelatedPrefab != null)
            {
                var database = receiversPlacemat.RelatedPrefab.GetComponent<ReferencesDatabase>();
                database.AddEvent(eventData);
            }
            RuntimeUtility.FindRelatedRuntimeOnActiveScene((GraphAssetModel)AssetModel).ReferencesDatabase
                .AddEvent(eventData);
        }

        public override void OnDisconnection(IPortModel selfConnectedPortModel, IPortModel otherConnectedPortModel)
        {
            
        }

        protected override void OnDefineNode()
        {
            base.OnDefineNode();

            if (string.IsNullOrEmpty(MonoScriptGuid))
                return;

            Serializer = new ScriptSerializer((ScriptGraphModel)m_AssetModel.GraphModel,
                PersistentGuid, MonoScriptGuid, RuntimeUtility.FindRelatedRuntimeOnActiveScene((GraphAssetModel)AssetModel));
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
                        placematModel.ContainingNodes.Add(PersistentGuid);
                        ContainingGameObjectPlacemat = placematModel.Guid;
                    }
                    else
                    {
                        if (placematModel.ContainingNodes.Contains(Guid))
                        {
                            ContainingGameObjectPlacemat = default;
                            placematModel.ContainingNodes.Remove(PersistentGuid);
                        }
                    }
                }
            }
        }

    }
}
