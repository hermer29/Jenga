using System;
using System.Collections.Generic;
using UnityEditor;

namespace Jenga.Core
{
    public class ScriptInstance
    {
        public Type Type;
        public List<EventSubscription> Events;
        public GUID GUID;
        public SerializedField[] Fields;
    }

    public struct SerializedField
    {
        public string FieldName;
        public Type FieldsType;
    }
    
    public struct EventSubscription
    {
        public string FromEvent;
        public GUID TargetObjectGUID;
        public string TargetMethod;
    }
}