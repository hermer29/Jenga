using UnityEditor;
using UnityEngine.Device;

namespace Jenga.Runtime
{
    public partial class ReferencesDatabase
    {
#if UNITY_EDITOR
        public void AddObjectReference(string guid, UnityEngine.Object referenced)
        {
            objectReferences.Add(guid, referenced);
        }

        public void DeleteObject(string guid)
        {
            Destroy(objectReferences[guid.ToString()]);
            objectReferences.Remove(guid.ToString());
        }

        public UnityEngine.Object GetObjectReferenceByGuid(GUID guid)
        {
            objectReferences.TryGetValue(guid.ToString(), out var result);
            return result;
        }

        public string? GetGuidByObjectReference(UnityEngine.Object obj)
        {
            foreach (var (key, value) in objectReferences)
            {
                if (value == obj)
                {
                    return key;
                }
            }

            return null;
        }

        public void RemoveObjectReference(string guid)
        {
            objectReferences.Remove(guid.ToString());
        }
        
        public void AddEvent(NodeEvent evt)
        {
            events.Add(evt);
            if (Application.isPlaying)
            {
                EventRegisterRequested.Invoke(evt);
            }
        }

        public void RemoveEvent(NodeEvent evt)
        {
            events.Remove(evt);
        }
#endif
    }
}