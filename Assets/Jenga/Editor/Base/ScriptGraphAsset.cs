using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Callbacks;
using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;
using UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts.UI;
using Jenga.Core;
using Jenga.Editor.Utility;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts
{
    [Serializable]
    public class ScriptGraphAsset : GraphAssetModel, IScriptGraphProvider
    {
        protected override Type GraphModelType => typeof(ScriptGraphModel);

        public static ScriptGraphAsset LastOpenedAsset
        {
            get
            {
                var path = SessionState.GetString("LastOpenedJengaGraph", "");
                return AssetDatabase.LoadAssetAtPath<ScriptGraphAsset>(path);
            }
            set => SessionState.SetString("LastOpenedJengaGraph", AssetDatabase.GetAssetPath(value));
        }

        [MenuItem("Window/Jenga Graph")]
        public static void CreateGraph(MenuCommand menuCommand)
        {
            const string path = "Assets";
            var template = new GraphTemplate<ScriptGraphStencil>(ScriptGraphStencil.GraphName);

            CommandDispatcher commandDispatcher = null;
            if (EditorWindow.HasOpenInstances<JengaGraphViewWindow>())
            {
                var window = EditorWindow.GetWindow<JengaGraphViewWindow>();
                if (window != null)
                {
                    commandDispatcher = window.CommandDispatcher;
                }
            }

            GraphAssetCreationHelpers<ScriptGraphAsset>.CreateInProjectWindow(template, commandDispatcher, path);
        }

        [OnOpenAsset(1)]
        public static bool OpenGraphAsset(int instanceId, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceId);
            if (obj is ScriptGraphAsset graphAssetModel)
            {
                var window = GraphViewEditorWindow.FindOrCreateGraphWindow<JengaGraphViewWindow>();
                LastOpenedAsset = graphAssetModel;
                window.SetCurrentSelection(graphAssetModel, GraphViewEditorWindow.OpenMode.OpenAndFocus);
                return window != null;
            }

            return false;
        }

        Graph IScriptGraphProvider.GetGraph()
        {
            var model = GraphModel as ScriptGraphModel;
            var graph = new Graph();
            graph.NodesByGUID = new Dictionary<string, ScriptInstance>();
            graph.ScriptNodes = model.NodeModels.Where(fNode => fNode is ScriptNodeModel).Select(node =>
            {
                var scriptNode = node as ScriptNodeModel;
                var script = new ScriptInstance
                {
                    Type = AssetUtility.FindTypeByGUID(scriptNode.MonoScriptGuid),
                    Events = new List<EventSubscription>(),
                    GUID = scriptNode.Guid.ToGUID()
                };
                foreach (var edge in scriptNode.GetConnectedEdges())
                {
                    if(scriptNode.Guid == (edge.ToPort.NodeModel as ScriptNodeModel).Guid)
                        continue;
                    script.Events.Add(new EventSubscription
                    {
                        FromEvent = edge.FromPort.UniqueName,
                        TargetMethod = edge.ToPort.UniqueName,
                        TargetObjectGUID = edge.ToPort.NodeModel.Guid.ToGUID()
                    });
                }

                graph.NodesByGUID.Add(scriptNode.Guid.ToString(), script);

                return script;
            }).ToArray();
            return graph;
        }

        private IEnumerable<ScriptNodeModel> GetConnectedNodes(ScriptNodeModel node)
        {
            foreach (var connectedEdge in node.GetConnectedEdges())
            {
                yield return connectedEdge.ToPort.NodeModel as ScriptNodeModel;
            }
        }
    }

    
}
