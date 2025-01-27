using UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts.UI.BaseModelUIParts;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts.UI.NodeViews
{
    public class ScriptNodeView : CollapsibleInOutNode
    {
        public static readonly string paramContainerPartName = "parameter-container";
        
        protected override void BuildPartList()
        {
            base.BuildPartList();

            PartList.InsertPartAfter(titleIconContainerPartName,
                new GameObjectInspectorFieldUIPart(paramContainerPartName, Model, this, ussClassName));
        }
    }
}