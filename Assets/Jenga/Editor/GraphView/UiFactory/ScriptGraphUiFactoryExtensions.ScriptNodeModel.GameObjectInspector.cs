using UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts.UI.BaseModelUIParts;
using UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts.UI.NodeViews;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts.UI.UiFactory
{
    [GraphElementsExtensionMethodsCache(typeof(ContextGraphView))]
    public static partial class ScriptGraphUiFactoryExtensions
    {
        public static IModelUI CreateNode(this ElementBuilder elementBuilder, CommandDispatcher store,
            ScriptNodeModel model)
        {
            IModelUI ui = new ScriptNodeView();
            ui.SetupBuildAndUpdate(model, store, elementBuilder.View, elementBuilder.Context);
            return ui;
        }
    }
}