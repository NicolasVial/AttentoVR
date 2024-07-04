using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TaskTableModifier : MonoBehaviour
{
    [SerializeField] private InputActionReference taskTableMoveInput;

    [SerializeField] private Transform[] transforms;

    [SerializeField] private GameObject rightArmHintGOFemale;
    [SerializeField] private GameObject rightArmHintGOMale;
    [SerializeField] private GameObject elbowSphere;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rightArmHintGOFemale.transform.position = elbowSphere.transform.position;
        rightArmHintGOMale.transform.position = elbowSphere.transform.position;
    }

    private void Awake()
    {
        taskTableMoveInput.action.performed += TaskTableMove;
    }

    private void OnDestroy()
    {
        taskTableMoveInput.action.performed -= TaskTableMove;
    }

    private void TaskTableMove(InputAction.CallbackContext context)
    {
        Vector2 val = context.action.ReadValue<Vector2>();
        float x = val.x;
        float y = val.y;
        if(Mathf.Abs(x) > Mathf.Abs(y))
        {
            if(x > 0)
            {
                MoveRight();
            }
            else
            {
                MoveLeft();
            }
        }
        else
        {
            if(y > 0)
            {
                MoveUp();
            }
            else
            {
                MoveDown();
            }
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

}
