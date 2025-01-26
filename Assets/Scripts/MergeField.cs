using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class MergeField : MonoBehaviour
    {

        public GameObject Prefab;
        
        public event Action<MergeElement> MergeObjectFactorized;
        
        private void Start()
        {
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    MergeObjectFactorized?.Invoke(CreateElement(new Vector2Int(x, y)));
                }
            }
        }

        private MergeElement CreateElement(Vector2Int vector2Int)
        {
            return null;
        }
    }
}