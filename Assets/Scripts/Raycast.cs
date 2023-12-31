using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    CubeInfo cubeInfo;

    void Start()
    {
        cubeInfo = FindObjectOfType<CubeInfo>();
    }

    void Update()
    {
        Ray rayDown = new(transform.position, Vector3.down);
        Ray rayLeft = new(transform.position, Vector3.left);

        RaycastHit hit;

        if (Physics.Raycast(rayDown, out hit)) //Useful to get the "new top face"
            cubeInfo.normalUp = hit.normal;

        if (Physics.Raycast(rayLeft, out hit))
            cubeInfo.normalRight = hit.normal;
    }
}