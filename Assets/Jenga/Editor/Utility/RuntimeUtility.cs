using Jenga.Runtime;
using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEngine;

namespace Jenga.Core.Utilities
{
    public static class RuntimeUtility
    {
        public static JengaRuntime FindRelatedRuntimeOnActiveScene(GraphAssetModel graphAssetModel)
        {
            foreach (var jengaRuntime in MonoBehaviour.FindObjectsOfType<JengaRuntime>())
            {
                if (jengaRuntime.obj == graphAssetModel)
                {
                    return jengaRuntime;
                }
            }

            return null;
        }
    }
}