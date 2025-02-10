using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts.UI;

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