using System.Linq;
using Jenga.Editor.Features.EditScriptContextMenu;
using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts;
using UnityEngine.UIElements;

namespace Jenga.Editor.ScriptNode
{
    public class ScriptNode : CollapsibleInOutNode
    {
        protected override void BuildPartList()
        {
            base.BuildPartList();
        }

        protected override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Edit Script", menuAction =>
            {
                var selection = GraphView.GetSelection()
                    .OfType<ScriptNodeModel>()
                    .FirstOrDefault();
                if (selection == null)
                    return;
                
                CommandDispatcher.Dispatch(new EditScriptCommand(selection));
            });
            
            base.BuildContextualMenu(evt);
        }
    }
}