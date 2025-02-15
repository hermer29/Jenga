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
    }
}
#endif