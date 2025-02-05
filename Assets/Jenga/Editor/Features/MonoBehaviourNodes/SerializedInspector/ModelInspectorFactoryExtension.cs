using UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts;

namespace UnityEditor.GraphToolsFoundation.Overdrive
{
    [GraphElementsExtensionMethodsCache(typeof(ModelInspectorView))]
    public static class ModelInspectorFactoryExtension
    {
        public static IModelUI CreateNodeInspector(this ElementBuilder elementBuilder, CommandDispatcher commandDispatcher, ScriptNodeModel model)
        {
            var ui = new ModelInspector();
            ui.Setup(model, commandDispatcher, elementBuilder.View as ModelInspectorView, elementBuilder.Context);

            var nodeInspectorFields = new ScriptEditorInspector("node-fields", model, ui, ModelInspector.ussClassName);
            ui.PartList.AppendPart(nodeInspectorFields);

            ui.BuildUI();
            ui.UpdateFromModel();

            return ui;
        }
    }
}
