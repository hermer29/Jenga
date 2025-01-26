using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProcessing : MonoBehaviour
{
    public event Action<Vector2> SomeEvent;

    private static int id;
    public int ourid;
    
    private void Awake()
    {
        ourid = id++;
    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        var inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        SomeEvent?.Invoke(inputVector);
    }

}
