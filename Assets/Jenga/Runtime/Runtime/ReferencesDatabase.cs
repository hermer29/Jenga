using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jenga.Core;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace Jenga.Runtime
{
    public partial class ReferencesDatabase : MonoBehaviour
    {
        public class NodesSendersEvent
        {
            public HashSet<string> ReceiversGuids = new HashSet<string>();
            public UnityEngine.Object Obj;
        }

        [SerializeField]
        private List<NodeEvent> events;

        [SerializeField] 
        private SerializableDictionary<SerializableGUIDJengaVersion, UnityEngine.Object> objectReferences;

        private static event Action<NodeEvent> EventRegisterRequested;
        private static event Action<SerializableGUIDJengaVersion, UnityEngine.Object> ObjectRegisterRequested;

        private static ReferencesDatabase Master;

        private HashSet<string> eventsInGame;
        private Dictionary<SerializableGUIDJengaVersion, List<UnityEngine.Object>> componentsInGame;

        public void Awake()
        {
            if (Master == null)
            {
                Master = this;
                eventsInGame = new HashSet<string>();
                componentsInGame = new Dictionary<SerializableGUIDJengaVersion, List<UnityEngine.Object>>();
                EventRegisterRequested += HandleRegisteringEvent;
                ObjectRegisterRequested += HandleRegisteringObject;
            }

            foreach (var (key, value) in objectReferences)
            {
                ObjectRegisterRequested!.Invoke(key, value);
            }
            
            foreach (var nodeEvent in events)
            {
                EventRegisterRequested!.Invoke(nodeEvent);
            }
        }

        private void HandleRegisteringEvent(NodeEvent nodeEvent)
        {
            if (componentsInGame.ContainsKey(nodeEvent.SendersGuid) && componentsInGame.ContainsKey(nodeEvent.ReceiversGuid))
            {
                var unitedGuid = SerializableGUIDJengaVersion.Generate(nodeEvent.SendersGuid, nodeEvent.ReceiversGuid) 
                                 + nodeEvent.SendersEventName + nodeEvent.ReceiversMethodName;
                
                if (!eventsInGame.Contains(unitedGuid))
                {
                    eventsInGame.Add(unitedGuid);
                    SubscribeMethodToEvent(nodeEvent);
                }
            }
        }

        private void SubscribeMethodToEvent(NodeEvent nodeEvent)
        {
            var senderObject = componentsInGame[nodeEvent.SendersGuid].First();
            var receiverObject = componentsInGame[nodeEvent.ReceiversGuid];
            var senderType = senderObject.GetType();
            EventInfo eventInfo = senderType.GetEvent(nodeEvent.SendersEventName);
            Assert.IsNotNull(eventInfo, "eventInfo != null");
            Type receiverType = receiverObject.GetType();
            MethodInfo methodInfo = receiverType.GetMethod(nodeEvent.ReceiversMethodName);
            Assert.IsNotNull(methodInfo, "methodInfo != null");
            Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, receiverObject, methodInfo);
            eventInfo.AddEventHandler(senderObject, handler);
        }

        private void HandleRegisteringObject(SerializableGUIDJengaVersion guid, UnityEngine.Object obj)
        {
            if (!componentsInGame.ContainsKey(guid))
            {
                componentsInGame.Add(guid, new List<Object>());
            }
            componentsInGame[guid].Add(obj);
        }
    }
}