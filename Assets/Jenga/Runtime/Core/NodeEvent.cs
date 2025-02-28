#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Jenga.Runtime
{
    [Serializable]
    public class NodeEvent
    {
        public string SendersGuid;
        public string ReceiversGuid;
        public string ReceiversMethodName;
        public string SendersEventName;

        public GameObject SendersPrefab;
        public GameObject ReceiversPrefab;
    }
}
#endif