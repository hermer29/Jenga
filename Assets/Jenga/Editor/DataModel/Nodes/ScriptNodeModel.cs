using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
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
        GameObject[] m_GameObjects;
        
        public GameObject[] GameObjects
        {
            get => m_GameObjects;
            set => m_GameObjects = value;
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

            m_GameObjects = new GameObject[GetGameObjectFields().Count()];
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

        private MethodInfo[] GetPublicMethods(Type type)
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

        public IEnumerable<FieldInfo> GetGameObjectFields()
        {
            var type = Type.GetType(assemblyQualifiedName);
            var result = new List<FieldInfo>();
            foreach (var fieldInfo in GetFields(type))
            {
                var fieldType = fieldInfo.FieldType;
                
                if (fieldType == typeof(GameObject))
                {
                    result.Add(fieldInfo);
                }
            }

            return result;
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
