using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;
using UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts;
using UnityEngine.UIElements;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts
{
    public class JengaGraphOnboardingProvider : OnboardingProvider
    {
        public override VisualElement CreateOnboardingElements(CommandDispatcher store)
        {
            var template = new GraphTemplate<ScriptGraphStencil>(ScriptGraphStencil.GraphName);
            return AddNewGraphButton<ScriptGraphAsset>(template);
        }
    }
}
