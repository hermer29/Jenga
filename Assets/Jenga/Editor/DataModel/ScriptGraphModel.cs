using System;
using System.Collections.Generic;
using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts
{
    [Serializable]
    public class ScriptGraphModel : GraphModel
    {
        public ScriptGraphModel()
        {
            StencilType = null;
        }

        public override Type DefaultStencilType => typeof(ScriptGraphStencil);
    }
}
