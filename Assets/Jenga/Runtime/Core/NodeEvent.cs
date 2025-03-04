#if UNITY_EDITOR
using System;
using Jenga.Core;
using UnityEngine;

namespace Jenga.Runtime
{
    [Serializable]
    public class NodeEvent
    {
        public SerializableGUIDJengaVersion SendersGuid;
        public SerializableGUIDJengaVersion ReceiversGuid;
        public string ReceiversMethodName;
        public string SendersEventName;

        public GameObject SendersPrefab;
        public GameObject ReceiversPrefab;
    }
}
#endif