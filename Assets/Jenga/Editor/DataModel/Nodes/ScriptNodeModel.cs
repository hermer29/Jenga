using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;
using UnityEngine;
using UnityEngine.GraphToolsFoundation.Overdrive;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts
{
    [Serializable]
    [SearcherItem(typeof(ScriptGraphStencil), SearcherContext.Graph, "")]
    public class ScriptNodeModel : NodeModel
    {
        [SerializeField, HideInInspector] 
        public string assemblyQualifiedName;
        
        [SerializeField] 
        GameObject m_Prefab;
        
        public GameObject Prefab
        {
            get => m_Prefab;
            set => m_Prefab = value;
        }

        public ScriptNodeModel()
        {
            Title = "Context Horizontal";
        }

        public void Initialize(Type type)
        {
            assemblyQualifiedName = type.AssemblyQualifiedName;
            Title = type.Name;
        }

        protected override void OnDefineNode()
        {
            base.OnDefineNode();

            if (string.IsNullOrEmpty(assemblyQualifiedName))
                return;
            
            var type = Type.GetType(assemblyQualifiedName);
            Title = type.Name;

            foreach (var methodInfo in GetPublicMethods(type))
            { 
                this.AddDataInputPort(methodInfo.Name, TypeHandle.Float);
            }
            
            // foreach (var fieldInfo in GetFields(type))
            // {
            //     var fieldType = fieldInfo.FieldType;
            //     TypeHandle portType = TypeHandle.Float;
            //     if (fieldType == typeof(GameObject))
            //     {
            //         portType = TypeHandle.GameObject;
            //     }
            //     
            //     this.AddDataInputPort(fieldInfo.Name, portType);
            // }
            //
            foreach (var publicInstanceEvent in GetPublicInstanceEvents(type))
            {
                this.AddDataOutputPort(publicInstanceEvent.Name, TypeHandle.Float);
            }
        }
        
        public MethodInfo[] GetPublicMethods(Type type)
        {
            if (type == null)
            {
                Debug.LogError("Type cannot be null!");
                return Array.Empty<MethodInfo>();
            }

            // Get all public methods (only instance)
            MethodInfo[] methodInfos = type.GetMethods(
                BindingFlags.Public | 
                BindingFlags.Instance | BindingFlags.DeclaredOnly
            );

            return methodInfos.Where(x => !x.IsSpecialName).ToArray();
        }

        private FieldInfo[] GetFields(Type type)
        {
            if (type == null)
            {
                Debug.LogError("Type cannot be null!");
                return Array.Empty<FieldInfo>();
            }

            // Get all public instance events
            FieldInfo[] fieldInfos = type.GetFields(
                BindingFlags.Public | 
                BindingFlags.Instance |
                BindingFlags.NonPublic
            );
            return fieldInfos.Where(x => (x.IsPrivate && x.GetCustomAttribute<SerializeField>() != null) || x.IsPublic).ToArray();
        }
        
        public EventInfo[] GetPublicInstanceEvents(Type type)
        {
            if (type == null)
            {
                Debug.LogError("Type cannot be null!");
                return Array.Empty<EventInfo>();
            }

            // Get all public instance events
            EventInfo[] eventInfos = type.GetEvents(
                BindingFlags.Public | 
                BindingFlags.Instance | 
                BindingFlags.DeclaredOnly
            );
            return eventInfos;
        }
    }
}
