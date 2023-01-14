using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffle : MonoBehaviour
{

    private int numberRotation;

    private float deltaMouseX;
    private float deltaMouseY;
    private float lerp;

    private Vector3 TopLeft;
    private Vector3 BottomRight;

    private Quaternion rotStart;
    private Quaternion rotEnd;

    private Plane planeVertFrontFace;
    private Plane planeHoriFrontFace;

    private CubeInfo cubeInfo;
    private List<GameObject> Face;


    // Start is called before the first frame update
    void Start()
    {
        cubeInfo = GetComponent<CubeInfo>();
        Face = new List<GameObject>();
        numberRotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(numberRotation < cubeInfo.shuffleSize && !cubeInfo.isRotating && !cubeInfo.pause)
        {
            cubeInfo.onShuffle = true;

            float min = -0.5f + (float)(-0.5 * (cubeInfo.sizeCube - 2));
            float max = 0.5f + (float)(0.5 * (cubeInfo.sizeCube - 2));

            TopLeft = Camera.main.WorldToScreenPoint(new Vector3(min, max, min)); //The top left cube of the front face
            BottomRight = Camera.main.WorldToScreenPoint(new Vector3(max, min, min)); //The bottom right cube of the front face

            //Random clic
            Vector3 MouseA = new Vector3(Random.Range(TopLeft.x, BottomRight.x), Random.Range(BottomRight.y, TopLeft.y),0);
            Vector3 MouseB = new Vector3(Random.Range(TopLeft.x, BottomRight.x), Random.Range(BottomRight.y, TopLeft.y),0);
            deltaMouseX = MouseB.x - MouseA.x;
            deltaMouseY = MouseB.y - MouseA.y;

            Ray ray = Camera.main.ScreenPointToRay(MouseA);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Face.Clear();
                planeVertFrontFace = new Plane(new Vector3(-1,0,0), hit.point);
                planeHoriFrontFace = new Plane(new Vector3(0, 1, 0), hit.point);
                float angleRotation = Mathf.PI / 2;

                foreach (Transform child in cubeInfo.Rubik.transform)
                {
                    if (child != cubeInfo.Rubik.transform && child != cubeInfo.rotatePoint)
                    {
                        if (Mathf.Abs(planeVertFrontFace.GetDistanceToPoint(child.transform.position)) <= 0.5f && Mathf.Abs(deltaMouseY) > Mathf.Abs(deltaMouseX))
                            Face.Add(child.gameObject);

                        if (Mathf.Abs(planeHoriFrontFace.GetDistanceToPoint(child.transform.position)) <= 0.5f && Mathf.Abs(deltaMouseX) > Mathf.Abs(deltaMouseY))
                            Face.Add(child.gameObject);
                    }
                }

                foreach (GameObject cube in Face)
                    cube.transform.SetParent(cubeInfo.rotatePoint.transform, true);

                rotStart = cubeInfo.rotatePoint.transform.rotation;
                GetRotation(angleRotation);
                StartCoroutine(Rotate());
            }
        }
        else if(numberRotation >= cubeInfo.shuffleSize && !cubeInfo.isRotating)
        {
            cubeInfo.onShuffle = false;
        }
    }

    private void GetRotation(float angleRotation)
    {

        if (Mathf.Abs(deltaMouseY) > Mathf.Abs(deltaMouseX) && deltaMouseY < 0)
            rotEnd = new Quaternion(planeVertFrontFace.normal.x * Mathf.Sin(angleRotation / 2), planeVertFrontFace.normal.y * Mathf.Sin(angleRotation / 2), planeVertFrontFace.normal.z * Mathf.Sin(angleRotation / 2), Mathf.Cos(angleRotation / 2)).normalized * rotStart.normalized;
        else if (Mathf.Abs(deltaMouseY) > Mathf.Abs(deltaMouseX) && deltaMouseY > 0)
            rotEnd = new Quaternion(planeVertFrontFace.normal.x * Mathf.Sin(-angleRotation / 2), planeVertFrontFace.normal.y * Mathf.Sin(-angleRotation / 2), planeVertFrontFace.normal.z * Mathf.Sin(-angleRotation / 2), Mathf.Cos(-angleRotation / 2)).normalized * rotStart.normalized;
        else if (Mathf.Abs(deltaMouseX) > Mathf.Abs(deltaMouseY) && deltaMouseX > 0)
            rotEnd = new Quaternion(planeHoriFrontFace.normal.x * Mathf.Sin(angleRotation / 2), planeHoriFrontFace.normal.y * Mathf.Sin(angleRotation / 2), planeHoriFrontFace.normal.z * Mathf.Sin(angleRotation / 2), Mathf.Cos(angleRotation / 2)).normalized * rotStart.normalized;
        else if (Mathf.Abs(deltaMouseX) > Mathf.Abs(deltaMouseY) && deltaMouseX < 0)
            rotEnd = new Quaternion(planeHoriFrontFace.normal.x * Mathf.Sin(-angleRotation / 2), planeHoriFrontFace.normal.y * Mathf.Sin(-angleRotation / 2), planeHoriFrontFace.normal.z * Mathf.Sin(-angleRotation / 2), Mathf.Cos(-angleRotation / 2)).normalized * rotStart.normalized;

    }

    IEnumerator Rotate()
    {
        cubeInfo.isRotating = true;

        while (lerp < 1)
        {
            cubeInfo.rotatePoint.transform.rotation = Quaternion.Slerp(rotStart, rotEnd, lerp);
            lerp += cubeInfo.speedRotateFace * (1 + numberRotation * 0.1f) * Time.deltaTime;
            yield return null;
        }

        cubeInfo.rotatePoint.transform.rotation = rotEnd;
        lerp = 0;

        foreach (GameObject cube in Face)
        {
            if (cubeInfo.sizeCube % 2 == 1)
                cube.transform.localPosition = new Vector3(Mathf.Round(cube.transform.localPosition.x), Mathf.Round(cube.transform.localPosition.y), Mathf.Round(cube.transform.localPosition.z));
            else
                cube.transform.localPosition = new Vector3(Mathf.Round(cube.transform.localPosition.x * 10.0f) * 0.1f, Mathf.Round(cube.transform.localPosition.y * 10.0f) * 0.1f, Mathf.Round(cube.transform.localPosition.z * 10.0f) * 0.1f); ;

            cube.transform.SetParent(cubeInfo.Rubik.transform, true);
        };

        Face.Clear();
        numberRotation++;
        cubeInfo.isRotating = false;
    }

    public void StopCoroutines()
    {
        StopAllCoroutines();

        foreach (GameObject cube in Face)
            cube.transform.SetParent(cubeInfo.Rubik.transform, true);

        cubeInfo.rotatePoint.transform.rotation = rotEnd;
        numberRotation = 0;
        lerp = 0;
        Face.Clear();
    }


}
