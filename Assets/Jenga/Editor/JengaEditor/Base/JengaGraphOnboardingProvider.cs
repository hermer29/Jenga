using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;
using UnityEngine.UIElements;

namespace Jenga.Editor.Base
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
