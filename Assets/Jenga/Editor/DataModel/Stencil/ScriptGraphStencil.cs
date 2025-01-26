using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.GraphToolsFoundation.Overdrive;
using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;
using UnityEditor.GraphToolsFoundation.Searcher;
using UnityEngine;

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
            
            foreach (var type in FindAllMonoBehaviourTypes())
            {
                itemList.Add(new GraphNodeModelSearcherItem(GraphModel, null, t =>
                {
                    return t.CreateNode(typeof(ScriptNodeModel), initializationCallback: model =>
                    {
                        var scriptModel = (model as ScriptNodeModel);
                        scriptModel.Initialize(type);
                    });
                }, type.ToString()));
            }
            itemList.Add(new GraphNodeModelSearcherItem(GraphModel, null, t => t.CreateNode(typeof(PrefabReferenceNodeModel)), "Prefab Reference"));
            
            var database = new SearcherDatabase(itemList);
            m_Databases.Add(database);
        }
        
        private IEnumerable<Type> FindAllMonoBehaviourTypes()
        {
            // Get all loaded assemblies in the current AppDomain
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Find all types that inherit from MonoBehaviour
            List<Type> monoBehaviourTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    // Get all types in the assembly
                    Type[] types = assembly.GetTypes();

                    // Filter types that inherit from MonoBehaviour
                    IEnumerable<Type> derivedTypes = types.Where(t => t.IsSubclassOf(typeof(MonoBehaviour)));

                    // Add the filtered types to the list
                    monoBehaviourTypes.AddRange(derivedTypes);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    // Handle any exceptions that occur while loading types
                    Debug.LogWarning($"Failed to load types from assembly: {assembly.FullName}. Error: {ex.Message}");
                }
            }

            return monoBehaviourTypes;
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
