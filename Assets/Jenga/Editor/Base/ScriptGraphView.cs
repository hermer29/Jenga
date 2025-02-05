using UnityEngine;
namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts.UI
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
    }
}
