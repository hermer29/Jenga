using System;
using Jenga.Editor.Base;
using Jenga.Editor.Utility;
using Jenga.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.GraphToolsFoundation.Overdrive;

namespace Jenga.Editor.Features.ScriptsSerialization
{
    [Serializable]
    public class ScriptSerializer
    {
        private ScriptGraphModel model;
        private SerializableGUID guid;
        private string monoScriptGuid;
        private readonly JengaRuntime runtime;

        public ScriptSerializer(ScriptGraphModel model, SerializableGUID guid, string monoScriptGuid, JengaRuntime runtime)
        {
            this.model = model;
            this.guid = guid;
            this.monoScriptGuid = monoScriptGuid;
            this.runtime = runtime;
        }

        public UnityEngine.Object CreatedComponent => runtime.ReferencesDatabase
            .GetObjectReferenceByGuid<UnityEngine.Object>(guid.ToSerializableGUIDJengaVersion());
    }
}