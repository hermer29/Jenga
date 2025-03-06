using Jenga.Editor.Base;
using UnityEditor;
using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Jenga.Editor.Features.GameObjectGraphPresentation
{
    public class CreatePlacematGameObjectContextMenu
    {
        [MenuItem("GameObject/Reference In Jenga Graph", false, 10)]
        private static void ReferenceGameObjectInJengaGraph(MenuCommand menuCommand)
        {
            var jengaGraphViewWindow = EditorWindow.GetWindow<JengaGraphViewWindow>();
            var scriptGraphView = jengaGraphViewWindow.GraphView.GetFirstOfType<ScriptGraphView>();
            var worldCenter = scriptGraphView.contentRect.center;

            var localPoint = scriptGraphView.ContentViewContainer.WorldToLocal(worldCenter);

            Assert.IsNotNull(menuCommand, "menuCommand != null");
            Assert.IsNotNull(menuCommand.context, "menuCommand.context != null");
            
            var createPlacematCommand = new CreateGameObjectPlacematCommand(
                new Rect(localPoint, new Vector2(200, 200)), menuCommand.context.name, menuCommand.context);
            
            jengaGraphViewWindow
                .CommandDispatcher
                .Dispatch(createPlacematCommand);
        }
    }
}