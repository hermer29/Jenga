using System;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    public class Spawner : MonoBehaviour
    {
        public GameObject Prefab;
        public Transform Parent;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Instantiate(Prefab, Parent);
            }
        }
    }
}