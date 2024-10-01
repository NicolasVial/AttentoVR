using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmLineController : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform[] malePoints;
    [SerializeField] private Transform[] femalePoints;
    [SerializeField] private MenuManager menuManager;

    private Transform[] points;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetUpLine();
    }

    public void SetUpLine()
    {
        if (menuManager.avatarGender == MenuManager.AvatarGender.Male)
        {
            points = malePoints;
        }
        else
        {
            points = femalePoints;
        }
        lineRenderer.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, points[i].position);
        }
    }
}
