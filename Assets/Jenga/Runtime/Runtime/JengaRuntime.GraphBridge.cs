#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Jenga.Runtime
{
    public partial class JengaRuntime
    {
        [SerializeField] private SerializableDictionary<GUID, UnityEngine.Object> objectReferences;

        public void AddObjectReference(GUID guid, UnityEngine.Object referenced)
        {
            objectReferences.Add(guid, referenced);
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