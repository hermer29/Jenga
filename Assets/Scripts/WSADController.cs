using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class WSADController : MonoBehaviour
    {
        public float Speed = 1;

        public event Action SomeVvent;

        public void SomeMEthod()
        {
            
        }
        
        private void Update()
        {
            transform.Translate(Speed * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        }
    }
}