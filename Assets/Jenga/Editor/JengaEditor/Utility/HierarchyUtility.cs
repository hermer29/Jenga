using UnityEngine;

namespace Jenga.Editor.Utility
{
    public class HierarchyUtility
    {
        public static string GetGameObjectPathWithIndex(GameObject obj)
        {
            if (obj.transform.root == obj.transform)
                return ".";
            
            if (obj == null)
            {
                Debug.LogError("Объект не может быть null.");
                return string.Empty;
            }

            // Создаем путь, начиная с текущего объекта и двигаясь вверх по иерархии
            string path = "/" + obj.name + GetIndexInParent(obj);
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + GetIndexInParent(obj) + path;
            }

            return path;
        }

        // Метод для получения индекса объекта в родительской иерархии
        private static string GetIndexInParent(GameObject obj)
        {
            if (obj.transform.parent == null)
            {
                return ""; // Корневой объект не имеет индекса
            }

            // Получаем индекс объекта среди его siblings (объектов с тем же родителем)
            int index = obj.transform.GetSiblingIndex();
            return "[" + index + "]";
        }

        public static GameObject FindObjectByPath(string path, GameObject rootObject)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Путь не может быть пустым.");
                return null;
            }

            if (rootObject == null)
            {
                Debug.LogError("Корневой объект не может быть null.");
                return null;
            }

            if (path == ".")
                return rootObject;

            // Разделяем путь на части
            string[] parts = path.Split('/');

            // Начинаем с указанного корневого объекта
            GameObject currentObject = rootObject;

            // Проходим по каждой части пути
            foreach (string part in parts)
            {
                if (string.IsNullOrEmpty(part))
                    continue; // Пропускаем пустые части (например, если путь начинается с "/")

                // Извлекаем имя объекта и индекс
                string objectName = part;
                int index = 0;
                if (part.Contains("["))
                {
                    int bracketIndex = part.IndexOf('[');
                    objectName = part.Substring(0, bracketIndex);
                    string indexString = part.Substring(bracketIndex + 1, part.IndexOf(']') - bracketIndex - 1);
                    int.TryParse(indexString, out index);
                }

                // Ищем дочерний объект с указанным именем и индексом
                Transform child = FindChildByNameAndIndex(currentObject.transform, objectName, index);
                if (child == null)
                {
                    Debug.LogError($"Объект '{objectName}[{index}]' не найден в иерархии.");
                    return null;
                }
                currentObject = child.gameObject;
            }

            return currentObject;
        }

        // Метод для поиска дочернего объекта по имени и индексу
        private static Transform FindChildByNameAndIndex(Transform parent, string name, int index)
        {
            int currentIndex = 0;
            foreach (Transform child in parent)
            {
                if (child.name == name)
                {
                    if (currentIndex == index)
                    {
                        return child;
                    }

                    currentIndex++;
                }
            }

            return null;
        }
    }
}