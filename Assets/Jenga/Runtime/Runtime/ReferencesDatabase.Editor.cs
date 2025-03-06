using System;
using Jenga.Core;
using JetBrains.Annotations;
using UnityEngine.Assertions;
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

        [CanBeNull]
        public TObject GetObjectReferenceByGuid<TObject>(SerializableGUIDJengaVersion guid) where TObject: UnityEngine.Object
        {
            if (!objectReferences.TryGetValue(guid, out var result))
                return null;
            if (result == null)
            {
                objectReferences.Remove(guid);
                return null;
            }
            if (result is not TObject value)
                throw new ArgumentException($"{result.name} is not {typeof(TObject)}. It is {result.GetType()}");
            return value;
        }

        public SerializableGUIDJengaVersion GetGuidByObjectReference(UnityEngine.Object obj)
        {
            Assert.IsNotNull(obj, "obj != null");
            Assert.IsNotNull(objectReferences, "objectReferences != null");
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