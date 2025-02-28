using System;
using Jenga.Editor.Base;
using Jenga.Editor.Utility;
using Jenga.Runtime;
using UnityEditor;
using UnityEngine;

namespace Jenga.Editor.Features.ScriptsSerialization
{
    [Serializable]
    public class ScriptSerializer
    {
        private ScriptGraphModel model;
        private GUID guid;
        private string monoScriptGuid;
        private readonly JengaRuntime runtime;

        public ScriptSerializer(ScriptGraphModel model, GUID guid, string monoScriptGuid, JengaRuntime runtime)
        {
            this.model = model;
            this.guid = guid;
            this.monoScriptGuid = monoScriptGuid;
            this.runtime = runtime;
        }

        public UnityEngine.Object CreatedComponent => runtime.ReferencesDatabase.GetObjectReferenceByGuid(guid);
    }
}