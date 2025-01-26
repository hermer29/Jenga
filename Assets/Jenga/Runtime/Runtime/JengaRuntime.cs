using System;
using System.Collections.Generic;
using Jenga.Runtime.Core;
using UnityEditor;
using UnityEngine;

namespace Jenga.Runtime
{
    public class JengaRuntime : MonoBehaviour
    {
        public ScriptableObject obj;

        private Dictionary<GUID, object> components = new Dictionary<GUID, object>();

        private void Awake()
        {
            if (obj is not IScriptGraphProvider graphProvider) 
                return;
            
            var graph = graphProvider.GetGraph();
            foreach (var scriptNode in graph.ScriptNodes)
            {
                var component = gameObject.AddComponent(scriptNode.Type);
                component.hideFlags = HideFlags.HideInInspector;
                components.Add(scriptNode.GUID, component);   
            }
                
            foreach (var graphScriptNode in graph.ScriptNodes)
            {
                foreach (var eventSubscription in graphScriptNode.Events)
                {
                    var publisherObject = components[graphScriptNode.GUID];
                    var subscriberObject = components[eventSubscription.TargetObjectGUID];
                    var @event = publisherObject.GetType().GetEvent(eventSubscription.FromEvent);
                    var method = subscriberObject.GetType().GetMethod(eventSubscription.TargetMethod);
                    Delegate targetMethodHandler = Delegate.CreateDelegate(@event.EventHandlerType, subscriberObject, method);
                    @event.AddEventHandler(publisherObject, targetMethodHandler);
                }
            }
            
            return;

        }
    }
}