using System;
using System.Collections.Generic;
using UnityEditor;

namespace Jenga.Runtime.Core
{
    public class ScriptNode
    {
        public string AssemblyQualifiedName;
        public Type Type;
        public List<EventSubscription> Events;
        public GUID GUID;
    }
    
    public struct EventSubscription
    {
        public string FromEvent;
        public GUID TargetObjectGUID;
        public string TargetMethod;
    }
}