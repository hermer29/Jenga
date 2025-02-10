using Jenga.Runtime;
using UnityEditor;
using UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts.UI;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class SelectionChangeCallbackExample
{
    [InitializeOnLoadMethod]
    private static void Initialize()
    {
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
            System.Type inspectorType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");

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