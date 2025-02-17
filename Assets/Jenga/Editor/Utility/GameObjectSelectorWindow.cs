namespace Jenga.Editor.Utility
{
    using UnityEngine;
    using UnityEditor;
    using System.Threading.Tasks;

    public class GameObjectSelectorWindow : EditorWindow
    {
        private GameObject selectedGameObject;
        private System.Threading.Tasks.TaskCompletionSource<GameObject> tcs;

        public static async Task<GameObject> ShowWindowAsync()
        {
            var window = GetWindow<GameObjectSelectorWindow>("Select GameObject");
            window.tcs = new TaskCompletionSource<GameObject>();
            return await window.tcs.Task;
        }

        private void OnGUI()
        {
            GUILayout.Label("Select a GameObject from the scene:", EditorStyles.boldLabel);

            selectedGameObject = (GameObject)EditorGUILayout.ObjectField("Selected GameObject", selectedGameObject, typeof(GameObject), true);

            if (GUILayout.Button("Apply"))
            {
                if (selectedGameObject != null)
                {
                    tcs.TrySetResult(selectedGameObject);
                    Close();
                }
                else
                {
                    Debug.LogWarning("No GameObject selected!");
                }
            }
        }

        private void OnDestroy()
        {
            tcs.TrySetResult(null);
        }
    }
}