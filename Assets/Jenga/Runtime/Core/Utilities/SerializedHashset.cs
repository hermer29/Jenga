namespace Jenga.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class SerializedHashset<T>
    {
        // Основной HashSet, который мы используем для хранения данных
        private HashSet<T> _hashSet = new HashSet<T>();

        // Список для сериализации данных
        [SerializeField]
        private List<T> _serializedList = new List<T>();

        // Конструктор
        public SerializedHashset()
        {
            // Инициализация HashSet из списка при создании объекта
            _hashSet = new HashSet<T>(_serializedList);
        }

        // Метод для добавления элемента
        public void Add(T item)
        {
            _hashSet.Add(item);
            UpdateSerializedList();
        }

        // Метод для удаления элемента
        public bool Remove(T item)
        {
            bool removed = _hashSet.Remove(item);
            if (removed)
            {
                UpdateSerializedList();
            }
            return removed;
        }

        // Метод для проверки наличия элемента
        public bool Contains(T item)
        {
            return _hashSet.Contains(item);
        }

        // Метод для очистки коллекции
        public void Clear()
        {
            _hashSet.Clear();
            UpdateSerializedList();
        }

        // Метод для обновления сериализуемого списка
        private void UpdateSerializedList()
        {
            _serializedList.Clear();
            _serializedList.AddRange(_hashSet);
        }

        // Метод для получения количества элементов
        public int Count => _hashSet.Count;

        // Метод для получения итератора (можно использовать в foreach)
        public IEnumerator<T> GetEnumerator()
        {
            return _hashSet.GetEnumerator();
        }

        // Метод для десериализации данных после загрузки
        public void OnAfterDeserialize()
        {
            _hashSet = new HashSet<T>(_serializedList);
        }

        // Метод для сериализации данных перед сохранением
        public void OnBeforeSerialize()
        {
            UpdateSerializedList();
        }
    }
}