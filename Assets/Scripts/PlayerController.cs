using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform leftLegInitTransform;
    [SerializeField] private Transform rightLegInitTransform;
    [SerializeField] private Transform leftLegTargetTransform;
    [SerializeField] private Transform rightLegTargetTransform;

    // Start is called before the first frame update
    void Start()
    {
        leftLegTargetTransform.position = leftLegInitTransform.position;
        rightLegTargetTransform.position = rightLegInitTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        leftLegTargetTransform.position = leftLegInitTransform.position;
        rightLegTargetTransform.position = rightLegInitTransform.position;
    }
}
