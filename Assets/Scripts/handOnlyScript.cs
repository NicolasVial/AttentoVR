using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handOnlyScript : MonoBehaviour
{
    [SerializeField] private GameObject parentGO;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private GameObject handGO;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = parentGO.transform.position + offset;
        this.transform.eulerAngles = parentGO.transform.eulerAngles + rotationOffset;
        handGO.transform.localPosition = new Vector3(handGO.transform.localPosition.x, 0.115f, handGO.transform.localPosition.z);
    }
}
