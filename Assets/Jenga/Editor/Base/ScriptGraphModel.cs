using System;
using Jenga.Core.Utilities.Reactivity;
using Jenga.Runtime;
using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;
using UnityEngine;

namespace Jenga.Editor.Base
{
    [Serializable]
    public class ScriptGraphModel : GraphModel
    {
        
        public ScriptGraphModel()
        {
            StencilType = null;
        }

        public override Type DefaultStencilType => typeof(ScriptGraphStencil);
        [SerializeField] public ReactiveProperty<JengaRuntime> CurrentRuntime = new ReactiveProperty<JengaRuntime>(null);
    }
}
