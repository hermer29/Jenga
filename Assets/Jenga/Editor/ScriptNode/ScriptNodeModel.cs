using System;
using System.Linq;
using System.Reflection;
using Jenga.Editor.Features.ScriptsSerialization;
using Jenga.Editor.Utility;
using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;
using UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts;
using UnityEngine;
using UnityEngine.GraphToolsFoundation.Overdrive;

namespace Jenga.Editor.ScriptNode
{
    [Serializable]
    [SearcherItem(typeof(ScriptGraphStencil), SearcherContext.Graph, "")]
    public class ScriptNodeModel : NodeModel
    {
        public string MonoScriptGuid;
        [SerializeField] public ScriptSerializer Serializer;
        public SerializableGUID NodesGuid;
        
        public ScriptNodeModel()
        {
            Title = "Context Horizontal";
        }

        public Type MonoScriptType => AssetUtility.GetMonoScriptType(MonoScriptGuid);

        public void Initialize(string scriptGuid, SerializableGUID nodesGuid)
        {
            NodesGuid = nodesGuid;
            MonoScriptGuid = scriptGuid;
            Title = MonoScriptType.Name;
            Debug.Log($"Nodes guid updated: {nodesGuid}");
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
                NodesGuid.ToGUID(), MonoScriptGuid);
            var type = MonoScriptType;
            Title = type.Name;

            foreach (var methodInfo in GetPublicMethods(type))
            { 
                this.AddDataInputPort(methodInfo.Name, TypeHandle.Float);
            }
            
            foreach (var publicInstanceEvent in GetPublicInstanceEvents(type))
            {
                this.AddDataOutputPort(publicInstanceEvent.Name, TypeHandle.Float);
            }
        }

        private MethodInfo[] GetPublicMethods(Type type)
        {
            if (type == null)
            {
                Debug.LogError("Type cannot be null!");
                return Array.Empty<MethodInfo>();
            }

            MethodInfo[] methodInfos = type.GetMethods(
                BindingFlags.Public | 
                BindingFlags.Instance | BindingFlags.DeclaredOnly
            );

            return methodInfos.Where(x => !x.IsSpecialName).ToArray();
        }
        
        public EventInfo[] GetPublicInstanceEvents(Type type)
        {
            if (type == null)
            {
                Debug.LogError("Type cannot be null!");
                return Array.Empty<EventInfo>();
            }

            EventInfo[] eventInfos = type.GetEvents(
                BindingFlags.Public | 
                BindingFlags.Instance | 
                BindingFlags.DeclaredOnly
            );
            return eventInfos;
        }
    }
}
