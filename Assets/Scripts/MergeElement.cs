using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class MergeElement : MonoBehaviour
    {
        public Color type;

        public void Initialize()
        {
            
        }
        
        private void OnMouseDrag()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var plane = new Plane(Vector3.back, transform.position);
            plane.Raycast(ray, out float enter);
            transform.position = ray.GetPoint(enter);
        }
    }
}