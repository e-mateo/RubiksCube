using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRubikCube : MonoBehaviour
{
    private float mouseX = 0;
    private float mouseY = 0;

    private CubeInfo cubeInfo;
    private bool turnCube;

    private void Start()
    {
        cubeInfo = FindObjectOfType<CubeInfo>();
    }

    private void Update()
    {
        if(!cubeInfo.onShuffle)
        {
            if (Input.GetMouseButtonDown(1) && !cubeInfo.isRotating)
            {
                cubeInfo.isRotating = true;
                turnCube = true;
            }
            else if (Input.GetMouseButton(1) && turnCube)
            {
                mouseX = -Input.GetAxis("Mouse X");
                mouseY = Input.GetAxis("Mouse Y");

                //Rotation around the normalUp axis
                transform.parent.rotation = new Quaternion(Mathf.Sin(mouseX * Mathf.Deg2Rad) / 2 * cubeInfo.normalUp.x, Mathf.Sin(mouseX * Mathf.Deg2Rad) / 2 * cubeInfo.normalUp.y, Mathf.Sin(mouseX * Mathf.Deg2Rad) / 2 * cubeInfo.normalUp.z, Mathf.Cos(mouseX * Mathf.Deg2Rad)) * transform.parent.rotation;
                //Rotation around the X axis
                transform.parent.rotation = new Quaternion(Mathf.Sin(mouseY * Mathf.Deg2Rad) / 2, 0, 0, Mathf.Cos(mouseY * Mathf.Deg2Rad)) * transform.parent.rotation;
            }
            else if (Input.GetMouseButtonUp(1) && turnCube)
            {
                turnCube = false;
                cubeInfo.isRotating = false;
            }
        }
    }
}