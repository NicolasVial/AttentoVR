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
    [SerializeField] private GameObject point1GO;
    [SerializeField] private GameObject point2GO;
    [SerializeField] private GameObject point3GO;

    private float diffAngleDegree = 0f;
    private float angleReductionDegree = 1f;
    private float actualAngle = 0f;

    private Vector3 point1;
    private Vector3 point2;
    private Vector3 point3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            point1 = rightHand.transform.position;
            point1GO.transform.position = point1;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            point2 = rightHand.transform.position;
            point2GO.transform.position = point2;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            point3 = rightHand.transform.position;
            point3GO.transform.position = point3;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            /* method with 3 points
            Vector2 center = GetCenterOfCircle(new Vector2(point1.x, point1.z), new Vector2(point2.x, point2.z), new Vector2(point3.x, point3.z));
            Vector3 center3D = new Vector3(center.x, rightHand.transform.position.y, center.y);
            Vector3 center3DRightHandDiff = center3D - rightHand.transform.position;
            elbowSphere.transform.position = center3D;
            elbowSpherePoint2.transform.position = elbowSphere.transform.position + new Vector3(1, 0, 0);
            */

            //method with 2 points
            Vector2 intersection = GetIntersection(new Vector2(point1.x, point1.z), new Vector2(point1.x, point1.z + 1), new Vector2(point2.x, point2.z), new Vector2(point2.x + 1, point2.z));
            elbowSphere.transform.position = new Vector3(intersection.x, rightHand.transform.position.y, intersection.y);
            elbowSpherePoint2.transform.position = elbowSphere.transform.position + new Vector3(1, 0, 0);

            //armPivot.transform.localEulerAngles = new Vector3(armPivot.transform.localEulerAngles.x, 0f, armPivot.transform.localEulerAngles.z);
        }

        //elbowSphere.transform.position = new Vector3(elbowSphere.transform.position.x, elbowSphere.transform.position.y, headset.transform.position.z + offset);
        //elbowSpherePoint2.transform.position = new Vector3(elbowSpherePoint2.transform.position.x, elbowSpherePoint2.transform.position.y, headset.transform.position.z + offset);
 
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

    //Given 2 2D vectors, find the coordinate of the point that is the intersection of the 2 lines formed by the 2 vectors.
    private Vector2 GetIntersection(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4)
    {
        float x1 = point1.x;
        float y1 = point1.y;
        float x2 = point2.x;
        float y2 = point2.y;
        float x3 = point3.x;
        float y3 = point3.y;
        float x4 = point4.x;
        float y4 = point4.y;

        float x12 = x1 - x2;
        float x34 = x3 - x4;
        float y12 = y1 - y2;
        float y34 = y3 - y4;

        float c = x1 * y2 - y1 * x2;
        float a = x3 * y4 - y3 * x4;

        float denominator = x12 * y34 - y12 * x34;

        float x = (c * x34 - x12 * a) / denominator;
        float y = (c * y34 - y12 * a) / denominator;

        return new Vector2(x, y);
    }

    //Given 3 points, find the coordinate of the center of the circle that passes through the 3 points.
    private Vector2 GetCenterOfCircle(Vector2 point1, Vector2 point2, Vector2 point3)
    {
        float x1 = point1.x;
        float y1 = point1.y;
        float x2 = point2.x;
        float y2 = point2.y;
        float x3 = point3.x;
        float y3 = point3.y;

        float x12 = x1 - x2;
        float x13 = x1 - x3;

        float y12 = y1 - y2;
        float y13 = y1 - y3;

        float y31 = y3 - y1;
        float y21 = y2 - y1;

        float x31 = x3 - x1;
        float x21 = x2 - x1;

        float sx13 = x1 * x1 - x3 * x3;
        float sy13 = y1 * y1 - y3 * y3;

        float sx21 = x2 * x2 - x1 * x1;
        float sy21 = y2 * y2 - y1 * y1;

        float f = ((sx13) * (x12) + (sy13) * (x12) + (sx21) * (x13) + (sy21) * (x13)) / (2 * ((y31) * (x12) - (y21) * (x13)));

        float g = ((sx13) * (y12) + (sy13) * (y12) + (sx21) * (y13) + (sy21) * (y13)) / (2 * ((x31) * (y12) - (x21) * (y13)));

        float c = -x1 * x1 - y1 * y1 - 2 * g * x1 - 2 * f * y1;

        float h = -g;
        float k = -f;
        float r = Mathf.Sqrt(h * h + k * k - c);

        return new Vector2(h, k);
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

    public Vector3 GetFixedPoint1()
    {
        return elbowSphere.transform.position;
    }

    public Vector3 GetFixedPoint2()
    {
        return elbowSpherePoint2.transform.position;
    }

}
