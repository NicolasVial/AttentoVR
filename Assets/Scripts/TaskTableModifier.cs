using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TaskTableModifier : MonoBehaviour
{

    [SerializeField] private Transform[] transforms;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
            MoveUp();
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            MoveDown();
        }
        if(Input.GetKey(KeyCode.W))
        {
            MoveForward();
        }
        if(Input.GetKey(KeyCode.S))
        {
            MoveBackward();
        }
    }

    private void MoveRight()
    {
        foreach(Transform t in transforms)
        {
            t.position += new Vector3(0.002f, 0, 0);
        }
    }

    private void MoveLeft()
    {
        foreach(Transform t in transforms)
        {
            t.position += new Vector3(-0.002f, 0, 0);
        }
    }

    private void MoveUp()
    {
        foreach(Transform t in transforms)
        {
            t.position += new Vector3(0, 0.002f, 0);
        }
    }

    private void MoveDown()
    {
        foreach(Transform t in transforms)
        {
            t.position += new Vector3(0, -0.002f, 0);
        }
    }

    private void MoveBackward()
    {
        foreach(Transform t in transforms)
        {
            t.position += new Vector3(0, 0, -0.002f);
        }
    }

    private void MoveForward()
    {
        foreach(Transform t in transforms)
        {
            t.position += new Vector3(0, 0, 0.002f);
        }
    }

}
