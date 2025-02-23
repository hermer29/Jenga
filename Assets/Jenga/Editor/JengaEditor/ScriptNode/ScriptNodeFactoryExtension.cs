using Jenga.Editor.Base;
using UnityEditor.GraphToolsFoundation.Overdrive;

namespace Jenga.Editor.ScriptNode
{
    [GraphElementsExtensionMethodsCache(typeof(ScriptGraphView))]
    public static partial class ScriptNodeFactoryExtension
    {
        public static IModelUI CreateNode(this ElementBuilder elementBuilder, CommandDispatcher store, ScriptNodeModel model)
        {
            IModelUI ui = new ScriptNode();
            ui.SetupBuildAndUpdate(model, store, elementBuilder.View, elementBuilder.Context);
            return ui;
        }
    }
}