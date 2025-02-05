using System;
using System.Linq;
using System.Reflection;
using Jenga.Editor.Utility;
using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;
using UnityEngine;
using UnityEngine.GraphToolsFoundation.Overdrive;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts
{
    [Serializable]
    [SearcherItem(typeof(ScriptGraphStencil), SearcherContext.Graph, "")]
    public class ScriptNodeModel : NodeModel
    {
        public string MonoScriptGuid;
        public ScriptableObject ScriptSerializedInstance;

        public ScriptNodeModel()
        {
            Title = "Context Horizontal";
        }

        public Type MonoScriptType => AssetUtility.FindTypeByGUID(MonoScriptGuid);

        public void Initialize(string scriptGuid, ScriptableObject scriptSerializedInstance)
        {
            ScriptSerializedInstance = scriptSerializedInstance;
            MonoScriptGuid = scriptGuid;
            Title = MonoScriptType.Name;
        }

        protected override void OnDefineNode()
        {
            base.OnDefineNode();

            if (string.IsNullOrEmpty(MonoScriptGuid))
                return;

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
