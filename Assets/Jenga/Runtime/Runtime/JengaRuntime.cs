using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Jenga.Core;
using Object = UnityEngine.Object;

namespace Jenga.Runtime
{
    [ExecuteAlways]
    public partial class JengaRuntime : MonoBehaviour
    {
        public ScriptableObject obj;
        [HideInInspector]
        public SerializableDictionary<string, UnityEngine.Object> components;

        private void Awake()
        {
            if (Application.isPlaying)
            {
                if (obj is not IScriptGraphProvider graphProvider) 
                    return;
                
                var graph = graphProvider.GetGraph();
                BindEvents(graph);
            }
            #if UNITY_EDITOR
            
            #endif
            
        }

        private void BindEvents(Graph graph)
        {
            foreach (var graphScriptNode in graph.ScriptNodes)
            {
                foreach (var eventSubscription in graphScriptNode.Events)
                {
                    var publisherObject = components[graphScriptNode.GUID.ToString()];
                    var subscriberObject = components[eventSubscription.TargetObjectGUID.ToString()];
                    var @event = publisherObject.GetType().GetEvent(eventSubscription.FromEvent);
                    var method = subscriberObject.GetType().GetMethod(eventSubscription.TargetMethod);
                    Delegate targetMethodHandler = Delegate.CreateDelegate(@event.EventHandlerType, subscriberObject, method);
                    @event.AddEventHandler(publisherObject, targetMethodHandler);
                }
            }
        }

        public IReadOnlyDictionary<string, UnityEngine.Object> GetAllComponents() => components;

        public UnityEngine.Object AddOrGetComponent<TComponent>(GUID nodeGuid) where TComponent : MonoBehaviour 
            => AddOrGetComponent(typeof(TComponent), nodeGuid);

        public UnityEngine.Object AddOrGetComponent(Type componentType, GUID nodeGuid)
        {
            if (components.TryGetValue(nodeGuid.ToString(), out var component))
            {
                if (component != null)
                {
                    Debug.Log($"Component existed on runtime object, returning it: {nodeGuid.ToString()}");
                    return component;
                }

                components.Remove(nodeGuid.ToString());
            }
            Debug.Log($"Adding component to runtime object, {nodeGuid.ToString()}");
            var newComponent = gameObject.AddComponent(componentType);
            components.Add(nodeGuid.ToString(), newComponent);
            return newComponent;
        }

        public void RemoveComponent(GUID nodeGuid)
        {
            if (!components.TryGetValue(nodeGuid.ToString(), out _))
                return;
            if (!Application.isPlaying)
            {
                DestroyImmediate(components[nodeGuid.ToString()]);
            }
            else
            {
                Destroy(components[nodeGuid.ToString()]);
            }
            components.Remove(nodeGuid.ToString());
        }
        
        
    }
}