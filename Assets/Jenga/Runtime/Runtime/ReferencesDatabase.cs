#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Jenga.Runtime
{
    public class ReferencesDatabase : MonoBehaviour
    {
        [SerializeField] 
        private SerializableDictionary<GUID, UnityEngine.Object> objectReferences;

        [SerializeField]
        private List<NodeEvent> events;

        public void AddObjectReference(GUID guid, UnityEngine.Object referenced)
        {
            objectReferences.Add(guid, referenced);
        }

        public void AddEvent(NodeEvent evt)
        {
            events.Add(evt);
        }

        public void RemoveEvent(NodeEvent evt)
        {
            events.Remove(evt);
        }

        public void DeleteObject(GUID guid)
        {
            Destroy(objectReferences[guid]);
            objectReferences.Remove(guid);
        }

        public UnityEngine.Object GetObjectReferenceByGuid(GUID guid)
        {
            objectReferences.TryGetValue(guid, out var result);
            return result;
        }

        public GUID? GetGuidByObjectReference(UnityEngine.Object obj)
        {
            foreach (var (key, value) in objectReferences)
            {
                if (value == obj)
                    return key;
            }

            return null;
        }

        public void RemoveObjectReference(GUID guid)
        {
            objectReferences.Remove(guid);
        }
    }
}
#endif