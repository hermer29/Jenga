using System;
using UnityEngine;

namespace Jenga.Core.Utilities.Reactivity
{
    public delegate void PropertyChanged<in T>(T oldValue, T newValue);
    
    [Serializable]
    public class ReactiveProperty<T>
    {
        [SerializeField] private T value;
        private PropertyChanged<T> changeSubscribers;

        public ReactiveProperty(T value)
        {
            this.value = value;
        }

        public void Subscribe(PropertyChanged<T> onChange)
        {
            changeSubscribers += onChange;
        }

        public T Value
        {
            get => value;
            set
            {
                changeSubscribers?.Invoke(this.value, value);
                this.value = value;
            }
        }
    }
}