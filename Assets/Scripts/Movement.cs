using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Movement : MonoBehaviour
    {
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                GetComponent<Rigidbody>().AddForce(Vector3.up * 1000);
        }
    }
}