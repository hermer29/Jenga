using System;
using UnityEditor;
using UnityEngine;

namespace Jenga.Editor.Utility
{
    public static class AssetUtility
    {
        public static string GetMonoScriptGUID(Type scriptType)
        {
            if (!scriptType.IsSubclassOf(typeof(MonoBehaviour)))
            {
                Debug.LogError($"Type {scriptType.Name} does not inherit from MonoBehaviour.");
                return null;
            }

            string[] guids = AssetDatabase.FindAssets("t:MonoScript");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                MonoScript monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(path);

                if (monoScript != null && monoScript.GetClass() == scriptType)
                {
                    return guid;
                }
            }

            Debug.LogWarning($"Script of type {scriptType.Name} not found in the project.");
            return null;
        }

        public static Type GetMonoScriptType(string guid)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning($"No asset found with GUID: {guid}");
                return null;
            }
            MonoScript monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
            if (monoScript == null)
            {
                Debug.LogWarning($"Asset at path {path} is not a MonoScript.");
                return null;
            }
            Type scriptType = monoScript.GetClass();
            if (scriptType == null)
            {
                Debug.LogWarning($"MonoScript at path {path} does not represent a valid MonoBehaviour type.");
                return null;
            }
            if (!scriptType.IsSubclassOf(typeof(MonoBehaviour)))
            {
                Debug.LogWarning($"Type {scriptType.Name} does not inherit from MonoBehaviour.");
                return null;
            }

            return scriptType;
        }
    }
}