using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class MergeField : MonoBehaviour
    {

        public MergeElement Prefab;
        public float Spacing;
        
        public event Action<MergeElement> MergeObjectFactorized;

        public event Action<int> ScoreCounterUpdated; 

        private void Start()
        {
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    var element = CreateElement(new Vector2Int(x, y));
                    MergeObjectFactorized?.Invoke(element);
                }
            }
        }

        private MergeElement CreateElement(Vector2Int vector2Int)
        {
            return Instantiate(Prefab, ((Vector2)vector2Int) * Spacing, Quaternion.identity);
        }
    }
}