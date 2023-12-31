using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCube : MonoBehaviour
{
    private int curSize;

    [SerializeField] private GameObject cube;
    private CubeInfo cubeInfo;

    void Start()
    {
        curSize = 0;
        cubeInfo = GetComponent<CubeInfo>();

        GameObject rubik = Instantiate(new GameObject("RubikCube"), new Vector3(0, 0, 0), Quaternion.identity);
        cubeInfo.Rubik = rubik;

        GameObject rotatePoint = Instantiate(new GameObject("RotatePoint"), Vector3.zero, Quaternion.identity, cubeInfo.Rubik.transform);
        cubeInfo.rotatePoint = rotatePoint;
    }

    void Update()
    {
        if (cubeInfo.sizeCube != curSize && !cubeInfo.pause)
        {
            float min = -0.5f + (float)(-0.5 * (cubeInfo.sizeCube - 2));
            float max = 0.5f + (float)(0.5 * (cubeInfo.sizeCube - 2));

            for (float i = min; i <= max; i += 1f)
            {
                for (float j = min; j <= max; j += 1f)
                {
                    for (float k = min; k <= max; k += 1f)
                    {
                        if (i == min || i == max || j == min || j == max || k == min || k == max) //Do not generate useless cubes in the middle
                        {
                            GameObject newCube = Instantiate(cube, new Vector3(i, j, k), Quaternion.identity);
                            newCube.transform.SetParent(cubeInfo.Rubik.transform, true);
                        }
                    }
                }
            }

            curSize = cubeInfo.sizeCube;
        }
    }

    public void DestroyCube()
    {
        cubeInfo.Rubik.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        cubeInfo.rotatePoint.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        foreach (Transform child in cubeInfo.Rubik.transform)
            if (child != cubeInfo.Rubik.transform && child != cubeInfo.rotatePoint.transform)
                Destroy(child.gameObject);

        foreach (Transform child in cubeInfo.rotatePoint.transform)
            if (child != cubeInfo.Rubik.transform && child != cubeInfo.rotatePoint.transform)
                Destroy(child.gameObject);

        curSize = 0;
    }
}
