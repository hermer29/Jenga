using Jenga.Editor.Base;
using Jenga.Runtime;
using UnityEditor;
using UnityEngine;

namespace Jenga.Editor.Features.SwitchBetweenEditorWindows
{
    public class SelectionChangeCallbackExample
    {
        public const bool IsEnabled = false;
        
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            if(IsEnabled)
                Selection.selectionChanged += OnSelectionChanged;
        }

        private static void OnSelectionChanged()
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject != null && selectedObject.TryGetComponent(out JengaRuntime jengaRuntime))
            {
                EditorWindow.FocusWindowIfItsOpen<JengaGraphViewWindow>();
                EditorWindow.GetWindow<JengaGraphViewWindow>().ExternalOnSelectionChange();
            }
            else
            {
                System.Type inspectorType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.InspectorWindow");

                // Find the currently open Inspector window
                EditorWindow inspectorWindow = EditorWindow.GetWindow(inspectorType);

                // Focus the Inspector window
                if (inspectorWindow != null)
                {
                    inspectorWindow.Focus();
                }
                else
                {
                    Debug.LogWarning("Inspector window not found.");
                }
            }
        }
    }
}