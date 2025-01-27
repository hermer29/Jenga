using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jenga.Runtime.Core;
using NUnit.Framework;
using UnityEditor.Callbacks;
using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;
using UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts.UI;
using UnityEngine;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts
{
    [Serializable]
    public class ScriptGraphAsset : GraphAssetModel, IScriptGraphProvider
    {
        protected override Type GraphModelType => typeof(ScriptGraphModel);

        [MenuItem("Window/Jenga Graph")]
        public static void CreateGraph(MenuCommand menuCommand)
        {
            const string path = "Assets";
            var template = new GraphTemplate<ScriptGraphStencil>(ScriptGraphStencil.GraphName);
            CommandDispatcher commandDispatcher = null;
            if (EditorWindow.HasOpenInstances<ContextGraphViewWindow>())
            {
                var window = EditorWindow.GetWindow<ContextGraphViewWindow>();
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
                var window = GraphViewEditorWindow.FindOrCreateGraphWindow<ContextGraphViewWindow>();
                window.SetCurrentSelection(graphAssetModel, GraphViewEditorWindow.OpenMode.OpenAndFocus);
                return window != null;
            }

            return false;
        }

        Graph IScriptGraphProvider.GetGraph()
        {
            var model = GraphModel as ScriptGraphModel;
            var graph = new Graph();
            graph.NodesByGUID = new Dictionary<string, ScriptNode>();
            graph.ScriptNodes = model.NodeModels.Where(fNode => fNode is ScriptNodeModel).Select(node =>
            {
                var scriptNode = node as ScriptNodeModel;
                var script = new ScriptNode
                {
                    AssemblyQualifiedName = scriptNode.assemblyQualifiedName,
                    Type = Type.GetType(scriptNode.assemblyQualifiedName),
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
