using UnityEngine;

namespace Jenga.Editor.Utility
{
    public class HierarchyUtility
    {
        public static GameObject FindObjectByPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Path is null or empty.");
                return null;
            }

            // Split the path into individual names
            string[] names = path.Split('/');

            // Start searching from the root of the scene
            Transform current = null;

            // Iterate through the names in the path
            foreach (string name in names)
            {
                if (current == null)
                {
                    // Find the root object with the first name
                    GameObject rootObject = GameObject.Find(name);
                    if (rootObject == null)
                    {
                        Debug.LogError($"Root object '{name}' not found in the scene.");
                        return null;
                    }
                    current = rootObject.transform;
                }
                else
                {
                    // Find the child with the next name
                    current = current.Find(name);
                    if (current == null)
                    {
                        Debug.LogError($"Child object '{name}' not found under '{current.name}'.");
                        return null;
                    }
                }
            }

            return current?.gameObject;
        
        }
        
        public static string GetObjectPath(GameObject obj)
        {
            if (obj == null)
            {
                Debug.LogError("GameObject is null.");
                return string.Empty;
            }

            // Start with the object's name
            string path = obj.name;

            // Traverse up the hierarchy
            Transform parent = obj.transform.parent;
            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }

            return path;
        }
    }
}