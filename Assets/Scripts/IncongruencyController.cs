using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class IncongruencyController : MonoBehaviour
{
    [SerializeField] private InputActionReference incongruencyInput;
    [SerializeField] private GameObject head;
    [SerializeField] private IKTargetFollowVRRig IKTargetFollowVRRig;
    [SerializeField] private TextMeshProUGUI incongruencyTxt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // update incugruency text and put it in centimeters
        incongruencyTxt.text = "Incongruency: " + (IKTargetFollowVRRig.rightHand.trackingPositionOffset.x * 100).ToString("F2") + " cm";
    }

    private void Awake()
    {
        incongruencyInput.action.performed += ApplyIncongruency;
    }

    private void OnDestroy()
    {
        incongruencyInput.action.performed -= ApplyIncongruency;
    }

    private void ApplyIncongruency(InputAction.CallbackContext context)
    {
        Vector2 val = context.action.ReadValue<Vector2>();
        float x = val.x;
        if (x > 0)
        {
            MoveHeadLeft();
        }
        else
        {
            MoveHeadRight();
        }
    }

    private void MoveHeadRight()
    {
        head.transform.position += new Vector3(0.0025f, 0, 0);
        IKTargetFollowVRRig.rightHand.trackingPositionOffset = new Vector3(IKTargetFollowVRRig.rightHand.trackingPositionOffset.x - 0.0025f, IKTargetFollowVRRig.rightHand.trackingPositionOffset.y, IKTargetFollowVRRig.rightHand.trackingPositionOffset.z);
    }

    private void MoveHeadLeft()
    {
        head.transform.position += new Vector3(-0.0025f, 0, 0);
        IKTargetFollowVRRig.rightHand.trackingPositionOffset = new Vector3(IKTargetFollowVRRig.rightHand.trackingPositionOffset.x + 0.0025f, IKTargetFollowVRRig.rightHand.trackingPositionOffset.y, IKTargetFollowVRRig.rightHand.trackingPositionOffset.z);
    }


}
