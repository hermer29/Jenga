using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Jenga.Core;
using Object = UnityEngine.Object;

namespace Jenga.Runtime
{
    [ExecuteAlways, RequireComponent(typeof(ReferencesDatabase))]
    public partial class JengaRuntime : MonoBehaviour
    {
        public ScriptableObject obj;
        public ReferencesDatabase ReferencesDatabase;

        private void OnValidate()
        {
            if (ReferencesDatabase == null)
            {
                ReferencesDatabase = GetComponent<ReferencesDatabase>();
            }
        }

        private void Awake()
        {
            if (Application.isPlaying)
            {
                if (obj is not IScriptGraphProvider graphProvider) 
                    return;
                
                var graph = graphProvider.GetGraph();
                //BindEvents(graph);
            }
            #if UNITY_EDITOR
            
            #endif
            
        }

        // private void BindEvents(Graph graph)
        // {
        //     foreach (var graphScriptNode in graph.ScriptNodes)
        //     {
        //         foreach (var eventSubscription in graphScriptNode.Events)
        //         {
        //             var publisherObject = components[graphScriptNode.GUID.ToString()];
        //             var subscriberObject = components[eventSubscription.TargetObjectGUID.ToString()];
        //             var @event = publisherObject.GetType().GetEvent(eventSubscription.FromEvent);
        //             var method = subscriberObject.GetType().GetMethod(eventSubscription.TargetMethod);
        //             Delegate targetMethodHandler = Delegate.CreateDelegate(@event.EventHandlerType, subscriberObject, method);
        //             @event.AddEventHandler(publisherObject, targetMethodHandler);
        //         }
        //     }
        // }
    }
}