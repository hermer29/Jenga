using System;
using Jenga.Editor.Base;
using Jenga.Editor.Utility;
using Jenga.Runtime;
using UnityEditor;
using UnityEngine;

namespace Jenga.Editor.Features.ScriptsSerialization
{
    [Serializable]
    public class ScriptSerializer : IDisposable
    {
        private ScriptGraphModel model;
        private GUID guid;
        private string monoScriptGuid;

        public ScriptSerializer(ScriptGraphModel model, GUID guid, string monoScriptGuid)
        {
            this.model = model;
            this.guid = guid;
            this.monoScriptGuid = monoScriptGuid;
        }
        
        public UnityEngine.Object CreatedComponent { get; private set; }
        
        public void Initialize()
        {
            CreateComponentIfNotExists(model.CurrentRuntime.Value);
            model.CurrentRuntime.Subscribe(OnChange);
        }

        private void CreateComponentIfNotExists(JengaRuntime currentRuntimeValue)
        {
            if (currentRuntimeValue != null)
            {
                var monoscriptType = AssetUtility.GetMonoScriptType(monoScriptGuid);
                CreatedComponent = currentRuntimeValue.AddOrGetComponent(monoscriptType, guid);
                Debug.Log($"Ensured that {CreatedComponent.GetType()} component existing on runtime object", currentRuntimeValue.gameObject);
                return;
            }
            Debug.Log($"Current runtime value equals null, because of that we're not creating component");
        }

        private void OnChange(JengaRuntime value, JengaRuntime newValue)
        {
            Debug.Log("Catch jenga runtime value updated event, trying to account it by creating component nearby", newValue.gameObject);
            CreateComponentIfNotExists(newValue);
        }

        public void Dispose()
        {
            if (model.CurrentRuntime.Value != null)
            {
                Debug.Log($"Removing component because node is removed: {AssetUtility.GetMonoScriptType(monoScriptGuid)}");
                model.CurrentRuntime.Value.RemoveComponent(guid);
            }
        }
    }
}