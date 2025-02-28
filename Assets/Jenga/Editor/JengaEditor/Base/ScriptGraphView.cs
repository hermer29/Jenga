using Jenga.Editor.Features.GameObjectGraphPresentation;
using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jenga.Editor.Base
{
    class ScriptGraphView : GraphView
    {
        JengaGraphViewWindow m_SimpleGraphViewWindow;

        public JengaGraphViewWindow window
        {
            get { return m_SimpleGraphViewWindow; }
        }

        public ScriptGraphView(JengaGraphViewWindow simpleGraphViewWindow, bool withWindowedTools, CommandDispatcher store) : base(simpleGraphViewWindow, store, "SimpleGraphView")
        {
            m_SimpleGraphViewWindow = simpleGraphViewWindow;
        }

        protected override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            
            evt.menu.AppendAction("Create GameObject Placemat", menuAction =>
            {
                Vector2 mousePosition = menuAction?.eventInfo?.mousePosition ?? Event.current.mousePosition;
                Vector2 graphPosition = ContentViewContainer.WorldToLocal(mousePosition);

                var placematRect = new Rect(graphPosition.x, graphPosition.y, 200, 200);
                CommandDispatcher.Dispatch(new CreateGameObjectPlacematCommand(placematRect));
            });
        }
    }
}
