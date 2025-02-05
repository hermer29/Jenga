using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Jenga.Core.Utilities
{
    public static class ReflectionUtility
    {
        public static IEnumerable<Type> FindAllMonoBehaviourTypes()
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
    }
}