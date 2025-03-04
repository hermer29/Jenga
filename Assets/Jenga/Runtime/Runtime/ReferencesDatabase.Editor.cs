using Jenga.Core;
using UnityEngine.Device;

namespace Jenga.Runtime
{
    public partial class ReferencesDatabase
    {
#if UNITY_EDITOR
        public void AddObjectReference(SerializableGUIDJengaVersion guid, UnityEngine.Object referenced)
        {
            objectReferences.Add(guid, referenced);
        }

        public void DeleteObject(SerializableGUIDJengaVersion guid)
        {
            Destroy(objectReferences[guid]);
            objectReferences.Remove(guid);
        }

        public UnityEngine.Object GetObjectReferenceByGuid(SerializableGUIDJengaVersion guid)
        {
            objectReferences.TryGetValue(guid, out var result);
            return result;
        }

        public SerializableGUIDJengaVersion GetGuidByObjectReference(UnityEngine.Object obj)
        {
            foreach (var (key, value) in objectReferences)
            {
                if (value == obj)
                {
                    return key;
                }
            }

            return default;
        }

        public void RemoveObjectReference(SerializableGUIDJengaVersion guid)
        {
            objectReferences.Remove(guid);
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