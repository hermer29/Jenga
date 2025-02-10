using System.Collections.Generic;
using Jenga.Runtime;
using UnityEditor;
using UnityEditor.GraphToolsFoundation.Overdrive;

namespace Jenga.Editor.Base
{
    internal class JengaGraphViewWindow : GraphViewEditorWindow
    {
        [InitializeOnLoadMethod]
        static void RegisterTool()
        {
            ShortcutHelper.RegisterDefaultShortcuts<JengaGraphViewWindow>(ScriptGraphStencil.GraphName);
        }

        [MenuItem("GTF/Samples/Contexts Editor")]
        public static void ShowWindow()
        {
            GetWindow<JengaGraphViewWindow>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            EditorToolName = "Graph Inspector";
        }

        public void ExternalOnSelectionChange()
        {
            OnSelectionChange();
        }
        
        private void OnSelectionChange()
        {
            if (Selection.activeGameObject != null)
            {
                if (Selection.activeGameObject.TryGetComponent<JengaRuntime>(out var runtime))
                {
                    if (runtime.obj == GraphView?.GraphModel?.AssetModel)
                    {
                        (GraphView.GraphModel as ScriptGraphModel).CurrentRuntime.Value = runtime;
                    }
                }
            }
            GetWindow<JengaGraphViewWindow>();
        }

        /// <inheritdoc />
        protected override GraphToolState CreateInitialState()
        {
            var prefs = Preferences.CreatePreferences(EditorToolName);
            return new ScriptGraphState(GUID, prefs);
        }

        protected override GraphView CreateGraphView()
        {
            return new ScriptGraphView(this, true, CommandDispatcher);
        }

        protected override BlankPage CreateBlankPage()
        {
            var onboardingProviders = new List<OnboardingProvider>();
            onboardingProviders.Add(new JengaGraphOnboardingProvider());

            return new BlankPage(CommandDispatcher, onboardingProviders);
        }

        protected override bool CanHandleAssetType(IGraphAssetModel asset)
        {
            return asset is ScriptGraphAsset;
        }
    }
}
