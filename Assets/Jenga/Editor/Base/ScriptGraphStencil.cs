using System;
using System.Collections.Generic;
using UnityEngine.GraphToolsFoundation.Overdrive;
using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;
using UnityEditor.GraphToolsFoundation.Searcher;
using Jenga.Core.Utilities;
using Jenga.Editor.Utility;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts
{
    class ScriptGraphStencil : Stencil, ISearcherDatabaseProvider
    {
        List<SearcherDatabaseBase> m_Databases = new List<SearcherDatabaseBase>();

        public override string ToolName => GraphName;

        public static string GraphName => "Jenga";

        public ScriptGraphStencil()
        {
            List<SearcherItem> itemList = new List<SearcherItem>();
            
            foreach (var monoBehaviourType in ReflectionUtility.FindAllMonoBehaviourTypes())
            {
                itemList.Add(new GraphNodeModelSearcherItem(GraphModel, null, t =>
                {
                    return t.CreateNode(typeof(ScriptNodeModel), initializationCallback: model =>
                    {
                        InitializeScriptNodeModel(model, monoBehaviourType);
                    });
                }, monoBehaviourType.Name));
            }
           
            var database = new SearcherDatabase(itemList);
            m_Databases.Add(database);
        }

        private static void InitializeScriptNodeModel(INodeModel model, Type monoBehaviourType)
        {
            var scriptModel = (model as ScriptNodeModel);
            var scriptSerializedInstance = ScriptableObject.CreateInstance(monoBehaviourType);
            scriptSerializedInstance.name = monoBehaviourType.Name;
            AssetDatabase.AddObjectToAsset(scriptSerializedInstance, ScriptGraphAsset.LastOpenedAsset);
            scriptModel.Initialize(AssetUtility.GetGUID(monoBehaviourType), scriptSerializedInstance);
        }

        /// <inheritdoc />
        public override IBlackboardGraphModel CreateBlackboardGraphModel(IGraphAssetModel graphAssetModel)
        {
            return new BlackboardGraphModel(graphAssetModel);
        }

        public override Type GetConstantNodeValueType(TypeHandle typeHandle)
        {
            return TypeToConstantMapper.GetConstantNodeType(typeHandle);
        }

        public override ISearcherDatabaseProvider GetSearcherDatabaseProvider()
        {
            return this;
        }

        List<SearcherDatabaseBase> ISearcherDatabaseProvider.GetGraphElementsSearcherDatabases(IGraphModel graphModel)
        {
            return m_Databases;
        }

        List<SearcherDatabaseBase> m_EmptyList = new List<SearcherDatabaseBase>();
        List<SearcherDatabaseBase> ISearcherDatabaseProvider.GetVariableTypesSearcherDatabases()
        {
            return m_EmptyList;
        }

        List<SearcherDatabaseBase> ISearcherDatabaseProvider.GetGraphVariablesSearcherDatabases(IGraphModel graphModel)
        {
            return m_Databases;
        }

        List<SearcherDatabaseBase> ISearcherDatabaseProvider.GetDynamicSearcherDatabases(IPortModel portModel)
        {
            return m_Databases;
        }

        public List<SearcherDatabaseBase> GetDynamicSearcherDatabases(IEnumerable<IPortModel> portModel)
        {
            return m_Databases;
        }
    }
}
