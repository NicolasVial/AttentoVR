using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class IncongruencyController : MonoBehaviour
{
    [SerializeField] private InputActionReference incongruencyInput;
    //[SerializeField] private IKTargetFollowVRRig IKTargetFollowVRRigMale;
    //[SerializeField] private IKTargetFollowVRRig IKTargetFollowVRRigFemale;
    [SerializeField] private GameObject elbowSphere;
    [SerializeField] private GameObject elbowSpherePoint2;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject armPivot;
    [SerializeField] private GameObject headset;
    [SerializeField] private float offset;

    private float diffAngleDegree = 0f;
    private float angleReductionDegree = 1f;
    private float actualAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        elbowSphere.transform.position = new Vector3(elbowSphere.transform.position.x, elbowSphere.transform.position.y, headset.transform.position.z + offset);
        elbowSpherePoint2.transform.position = new Vector3(elbowSpherePoint2.transform.position.x, elbowSpherePoint2.transform.position.y, headset.transform.position.z + offset);
 
        /*
         * Get the angle formed by the line formed by the elbowSphere and elbowSpherePoint2 and the line formed by the elbowSphere and the rightHandPos in the x,z plan.
         * The angle must run in an anticlockwise direction from the line formed by the elbowSphere and elbowSpherePoint2 to the line formed by the elbowSphere and the rightHandPos.
         */
        
        Vector2 elbowSphereToRightHand = new Vector2(rightHand.transform.position.x - elbowSphere.transform.position.x, rightHand.transform.position.z - elbowSphere.transform.position.z);
        Vector2 elbowSphereToElbowSpherePoint2 = new Vector2(elbowSpherePoint2.transform.position.x - elbowSphere.transform.position.x, elbowSpherePoint2.transform.position.z - elbowSphere.transform.position.z);
        float angle = Vector2.SignedAngle(elbowSphereToElbowSpherePoint2, elbowSphereToRightHand);
        angle -= 180;
        if (angle < 0)
        {
            angle += 180;
        }
        angle = 180 - angle;
        actualAngle = angle;

        /*
        //We now want to add an incongruency to the angle formed by the line formed by the elbowSphere and elbowSpherePoint2 and the line formed by the elbowSphere and the rightHandPos in the x,z plan. To apply the incongruency,
        //we need to find the values for the variable IKTargetFollowVRRigMale.rightHand.trackingRotationOffset.y and IKTargetFollowVRRigFemale.rightHand.trackingPositionOffset.
        Vector2 elbowSphereToIKTarget = new Vector2(rightHand.transform.position.x - elbowSphere.transform.position.x, rightHand.transform.position.z - elbowSphere.transform.position.z);
        float realX = rightHand.transform.position.x - elbowSphere.transform.position.x;
        float realZ = rightHand.transform.position.z - elbowSphere.transform.position.z;
        float incongruencyAngle = diffAngleDegree + angle - 90f;
        float incongruencyAngleRad = incongruencyAngle * Mathf.Deg2Rad;
        float incongruencyX = Mathf.Sin(incongruencyAngleRad) * elbowSphereToIKTarget.magnitude;
        float incongruencyZ = Mathf.Cos(incongruencyAngleRad) * elbowSphereToIKTarget.magnitude;
        float incongruencyXDiff = incongruencyX - realX;
        float incongruencyZDiff = incongruencyZ - realZ;
        IKTargetFollowVRRigMale.rightHand.trackingPositionOffset = new Vector3(incongruencyXDiff, -incongruencyZDiff, 0);
        IKTargetFollowVRRigFemale.rightHand.trackingPositionOffset = new Vector3(incongruencyXDiff, -incongruencyZDiff, 0);
        */

        float incongruencyAngle = diffAngleDegree + angle - 90f;
        armPivot.transform.localEulerAngles = new Vector3(armPivot.transform.localEulerAngles.x, incongruencyAngle, armPivot.transform.localEulerAngles.z);

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
        /*
        Vector2 val = context.action.ReadValue<Vector2>();
        float x = val.x;
        if (x > 0)
        {
            MakeAngleLarger();
        }
        else
        {
            MakeAngleSmaller();
        }
        */
    }

    private void MakeAngleSmaller()
    {
        diffAngleDegree -= angleReductionDegree;
    }

    private void MakeAngleLarger()
    {
        diffAngleDegree += angleReductionDegree;
    }

    public void SetIncongruencyAngle(float newDiffAngle)
    {
        diffAngleDegree = newDiffAngle;
    }

    public float GetAngle()
    {
        return actualAngle;
    }

    public float GetIncongruencyAngle()
    {
        return diffAngleDegree + actualAngle;    
    }

    public float GetDiffAngle()
    {
        return diffAngleDegree;
    }

}
