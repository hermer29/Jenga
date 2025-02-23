using Jenga.Editor.ScriptNode;
using UnityEditor;
using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEngine.GraphToolsFoundation.CommandStateObserver;

namespace Jenga.Editor.Features.EditScriptContextMenu
{
    public class EditScriptCommand : ICommand  
    {
        public readonly ScriptNodeModel ScriptNodeModel;

        public EditScriptCommand(ScriptNodeModel scriptNodeModel)
        {
            ScriptNodeModel = scriptNodeModel;
        }
        
        public static void DefaultCommandHandler(IState state, EditScriptCommand command)
        {
            OpenMonoScriptInCodeEditor(command);
        }

        private static void OpenMonoScriptInCodeEditor(EditScriptCommand command)
        {
            var monoScriptGuid = command.ScriptNodeModel.MonoScriptGuid;
            var assetPath = AssetDatabase.GUIDToAssetPath(monoScriptGuid);
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(assetPath, 0);
        }
    }
}