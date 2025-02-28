using Jenga.Editor.Base;
using UnityEditor;
using UnityEngine;

namespace Jenga.Editor.Features.GameObjectGraphPresentation
{
    public class CreatePlacematGameObjectContextMenu
    {
        [MenuItem("GameObject/Reference In Jenga Graph", false, 10)]
        private static void ReferenceGameObjectInJengaGraph(MenuCommand menuCommand)
        {
            var createPlacematCommand = new CreateGameObjectPlacematCommand(
                null, menuCommand.context.name, menuCommand.context);
            EditorWindow.GetWindow<JengaGraphViewWindow>()
                .CommandDispatcher
                .Dispatch(createPlacematCommand);
        }
    }
}