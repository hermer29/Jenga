using System;
using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts
{
    [Serializable]
    public class PrefabReferenceNodeModel : NodeModel
    {
        public PrefabReferenceNodeModel()
        {
            Title = "Prefab Reference";
        }
    }
}