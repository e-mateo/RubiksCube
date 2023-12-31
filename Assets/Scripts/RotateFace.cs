using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFace : MonoBehaviour
{

    private float mouseX;
    private float mouseY;
    private float lerp = 0;

    private Quaternion rotStart;
    private Quaternion rotEnd;

    private CubeInfo cubeInfo;
    private GameObject rubikCube;
    private List<GameObject> Face;

    public Vector3 normal;
    private Plane planeVertSideFaces;
    private Plane planeHoriSideFaces;

    private Plane planeForwardTopFace;
    private Plane planeRightTopFace;

    private bool clic;
    private bool clicUp;

    void Start()
    {
        Face = new List<GameObject>();
        cubeInfo = FindObjectOfType<CubeInfo>();
        rubikCube = transform.parent.gameObject;
    }

    private void GetNormalFaceClic(Vector3 mousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if(Physics.Raycast(ray, out RaycastHit hit))
            normal = hit.normal;
    }

    private void OnMouseDown()
    {
        if (!cubeInfo.isRotating && !clic && !clicUp &&!cubeInfo.onShuffle)
        {
            clic = true;
            cubeInfo.isRotating = true;
            GetNormalFaceClic(Input.mousePosition);

            //Clic on Sides faces
                //Rotation of 90 degree around the cubeInfo.normalUp axis of the normal vector
            Quaternion normalPlaneVert = new Quaternion(Mathf.Sin(Mathf.PI / 4) * cubeInfo.normalUp.x, Mathf.Sin(Mathf.PI / 4) * cubeInfo.normalUp.y, Mathf.Sin(Mathf.PI / 4) * cubeInfo.normalUp.z, Mathf.Cos(Mathf.PI / 4)) * new Quaternion(normal.x, normal.y, normal.z, 0) * Quaternion.Inverse(new Quaternion(Mathf.Sin(Mathf.PI / 4) * cubeInfo.normalUp.x, Mathf.Sin(Mathf.PI / 4) * cubeInfo.normalUp.y, Mathf.Sin(Mathf.PI / 4) * cubeInfo.normalUp.z, Mathf.Cos(Mathf.PI / 4)));
            planeVertSideFaces = new Plane(new Vector3(normalPlaneVert.x, normalPlaneVert.y, normalPlaneVert.z), transform.position);
            planeHoriSideFaces = new Plane(-cubeInfo.normalUp, transform.position);


            //Clic on Top or Bottom face
                //Rotation of 90 degree around the cubeInfo.normalRight axis of the normal vector
            Quaternion normalRightTopFace = new Quaternion(Mathf.Sin(Mathf.PI / 4) * cubeInfo.normalRight.x, Mathf.Sin(Mathf.PI / 4) * cubeInfo.normalRight.y, Mathf.Sin(Mathf.PI / 4) * cubeInfo.normalRight.z, Mathf.Cos(Mathf.PI / 4)) * new Quaternion(normal.x, normal.y, normal.z, 0) * Quaternion.Inverse(new Quaternion(Mathf.Sin(Mathf.PI / 4) * cubeInfo.normalRight.x, Mathf.Sin(Mathf.PI / 4) * cubeInfo.normalRight.y, Mathf.Sin(Mathf.PI / 4) * cubeInfo.normalRight.z, Mathf.Cos(Mathf.PI / 4)));
            planeRightTopFace = new Plane(new Vector3(normalRightTopFace.x, normalRightTopFace.y, normalRightTopFace.z), transform.position);
            planeForwardTopFace = new Plane(cubeInfo.normalRight, transform.position);
        }
    }

    private void OnMouseDrag()
    {
        if (cubeInfo.isRotating && clic && !clicUp && !cubeInfo.onShuffle)
        {
            mouseX += Input.GetAxis("Mouse X");
            mouseY += Input.GetAxis("Mouse Y");
        }
    }

    private void GetEndRotation(float angleRotation)
    {

        if (normal != cubeInfo.normalUp && normal != -cubeInfo.normalUp) //Sides faces
        {
            if (Mathf.Abs(mouseY) > Mathf.Abs(mouseX) && mouseY < 0)
                rotEnd = new Quaternion(planeVertSideFaces.normal.x * Mathf.Sin(angleRotation / 2), planeVertSideFaces.normal.y * Mathf.Sin(angleRotation / 2), planeVertSideFaces.normal.z * Mathf.Sin(angleRotation / 2), Mathf.Cos(angleRotation / 2)) * rotStart;
            else if (Mathf.Abs(mouseY) > Mathf.Abs(mouseX) && mouseY > 0)
                rotEnd = new Quaternion(planeVertSideFaces.normal.x * Mathf.Sin(-angleRotation / 2), planeVertSideFaces.normal.y * Mathf.Sin(-angleRotation / 2), planeVertSideFaces.normal.z * Mathf.Sin(-angleRotation / 2), Mathf.Cos(-angleRotation / 2)) * rotStart;
            else if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY) && mouseX > 0)
                rotEnd = new Quaternion(planeHoriSideFaces.normal.x * Mathf.Sin(angleRotation / 2), planeHoriSideFaces.normal.y * Mathf.Sin(angleRotation / 2), planeHoriSideFaces.normal.z * Mathf.Sin(angleRotation / 2), Mathf.Cos(angleRotation / 2)) * rotStart;
            else if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY) && mouseX < 0)
                rotEnd = new Quaternion(planeHoriSideFaces.normal.x * Mathf.Sin(-angleRotation / 2), planeHoriSideFaces.normal.y * Mathf.Sin(-angleRotation / 2), planeHoriSideFaces.normal.z * Mathf.Sin(-angleRotation / 2), Mathf.Cos(-angleRotation / 2)) * rotStart;
        }
        else //Top or Bottom face
        {
            if (Mathf.Abs(mouseY) > Mathf.Abs(mouseX) && mouseY > 0)
                rotEnd = new Quaternion(planeForwardTopFace.normal.x * Mathf.Sin(angleRotation / 2), planeForwardTopFace.normal.y * Mathf.Sin(angleRotation / 2), planeForwardTopFace.normal.z * Mathf.Sin(angleRotation / 2), Mathf.Cos(angleRotation / 2)) * rotStart;
            else if (Mathf.Abs(mouseY) > Mathf.Abs(mouseX) && mouseY < 0)
                rotEnd = new Quaternion(planeForwardTopFace.normal.x * Mathf.Sin(-angleRotation / 2), planeForwardTopFace.normal.y * Mathf.Sin(-angleRotation / 2), planeForwardTopFace.normal.z * Mathf.Sin(-angleRotation / 2), Mathf.Cos(-angleRotation / 2)) * rotStart;
            else if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY) && mouseX < 0)
                rotEnd = new Quaternion(planeRightTopFace.normal.x * Mathf.Sin(angleRotation / 2), planeRightTopFace.normal.y * Mathf.Sin(angleRotation / 2), planeRightTopFace.normal.z * Mathf.Sin(angleRotation / 2), Mathf.Cos(angleRotation / 2)) * rotStart;
            else if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY) && mouseX > 0)
                rotEnd = new Quaternion(planeRightTopFace.normal.x * Mathf.Sin(-angleRotation / 2), planeRightTopFace.normal.y * Mathf.Sin(-angleRotation / 2), planeRightTopFace.normal.z * Mathf.Sin(-angleRotation / 2), Mathf.Cos(-angleRotation / 2)) * rotStart;
        }
    }

    private void OnMouseUp()
    {
        float angleRotation = Mathf.PI / 2;

        if(cubeInfo.isRotating && clic && !clicUp && !cubeInfo.onShuffle)
        {
            clicUp = true;

            foreach (Transform child in transform.parent) // Get the all the cubes that will rotate
            {
                if (child != transform.parent && child != cubeInfo.rotatePoint)
                {
                    if (normal != cubeInfo.normalUp && normal != -cubeInfo.normalUp) //Sides faces
                    {
                        if (Mathf.Abs(planeVertSideFaces.GetDistanceToPoint(child.transform.position)) <= 0.5f && Mathf.Abs(mouseY) > Mathf.Abs(mouseX))
                            Face.Add(child.gameObject);
                        else if (Mathf.Abs(planeHoriSideFaces.GetDistanceToPoint(child.transform.position)) <= 0.5f && Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
                            Face.Add(child.gameObject);
                    }
                    else //Top or Bottom face
                    {
                        if (Mathf.Abs(planeForwardTopFace.GetDistanceToPoint(child.transform.position)) <= 0.5f && Mathf.Abs(mouseY) > Mathf.Abs(mouseX))
                            Face.Add(child.gameObject);
                        else if(Mathf.Abs(planeRightTopFace.GetDistanceToPoint(child.transform.position)) <= 0.5f && Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
                            Face.Add(child.gameObject);
                    }
                }
            }

            foreach (GameObject cube in Face)
                cube.transform.SetParent(cubeInfo.rotatePoint.transform, true);

            rotStart = cubeInfo.rotatePoint.transform.rotation;
            GetEndRotation(angleRotation);
            StartCoroutine(Rotate());

            mouseX = 0.0f;
            mouseY = 0.0f;
        }
    }

    IEnumerator Rotate()
    {
        while (lerp < 1)
        {
            cubeInfo.rotatePoint.transform.rotation = Quaternion.Slerp(rotStart, rotEnd, lerp);
            lerp += cubeInfo.speedRotateFace * Time.deltaTime;
            yield return null;
        }

        cubeInfo.rotatePoint.transform.rotation = rotEnd;
        lerp = 0;

        foreach (GameObject cube in Face)
        {
            if (cubeInfo.sizeCube % 2 == 1)
                cube.transform.localPosition = new Vector3((int)Mathf.Round(cube.transform.localPosition.x), (int)Mathf.Round(cube.transform.localPosition.y), (int)Mathf.Round(cube.transform.localPosition.z));
            else
                cube.transform.localPosition = new Vector3(Mathf.Round(cube.transform.localPosition.x * 10.0f) * 0.1f, Mathf.Round(cube.transform.localPosition.y * 10.0f) * 0.1f, Mathf.Round(cube.transform.localPosition.z * 10.0f) * 0.1f); ;

            cube.transform.SetParent(rubikCube.transform);
        };

        Face.Clear();
        cubeInfo.rotatePoint.transform.rotation = cubeInfo.Rubik.transform.rotation;
        cubeInfo.isSolved();

        cubeInfo.isRotating = false;
        clic = false;
        clicUp = false;
    }
}