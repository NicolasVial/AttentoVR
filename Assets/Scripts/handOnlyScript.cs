using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handOnlyScript : MonoBehaviour
{
    [SerializeField] private GameObject maleParentGO;
    [SerializeField] private GameObject femaleParentGO;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private GameObject handGO;

    public bool isMaleAvatar = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isMaleAvatar)
        {
            this.transform.position = maleParentGO.transform.position + offset;
            this.transform.eulerAngles = maleParentGO.transform.eulerAngles + rotationOffset;
            handGO.transform.localPosition = new Vector3(handGO.transform.localPosition.x, 0.115f, handGO.transform.localPosition.z);
        }
        else
        {
            this.transform.position = femaleParentGO.transform.position + offset;
            this.transform.eulerAngles = femaleParentGO.transform.eulerAngles + rotationOffset;
            handGO.transform.localPosition = new Vector3(handGO.transform.localPosition.x, 0.09f, handGO.transform.localPosition.z);
        }
    }
}
