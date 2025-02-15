using System;
using System.Collections.Generic;
using System.Linq;
using Jenga.Core.Utilities;
using Jenga.Editor.ScriptNode;
using Jenga.Editor.Utility;
using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;
using UnityEditor.GraphToolsFoundation.Searcher;
using UnityEngine.GraphToolsFoundation.Overdrive;

namespace Jenga.Editor.Base
{
    class ScriptGraphStencil : Stencil, ISearcherDatabaseProvider
    {
        List<SearcherDatabaseBase> m_Databases = new List<SearcherDatabaseBase>();

        public override string ToolName => GraphName;

        public static string GraphName => "Jenga";

        public ScriptGraphStencil()
        {
            List<SearcherItem> itemList = new List<SearcherItem>();
            
            foreach (var assemblyGrouping in ReflectionUtility.FindAllMonoBehaviourTypes().GroupBy(x => x.Assembly))
            {
                var parent = new SearcherItem(assemblyGrouping.Key.GetName().Name);
                foreach (var monoBehaviourType in assemblyGrouping)
                {
                    parent.AddChild(new GraphNodeModelSearcherItem(GraphModel, null, t =>
                    {
                        return t.CreateNode(typeof(ScriptNodeModel), initializationCallback: model =>
                        {
                            InitializeScriptNodeModel(model, monoBehaviourType);
                        });
                    }, monoBehaviourType.Name));
                }
                
                itemList.Add(parent);
            }
            
            var database = new SearcherDatabase(itemList);
            m_Databases.Add(database);
        }

        private void InitializeScriptNodeModel(INodeModel model, Type monoBehaviourType)
        {
            var scriptModel = (model as ScriptNodeModel);
            scriptModel.Initialize(AssetUtility.GetMonoScriptGUID(monoBehaviourType), SerializableGUID.Generate());
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
