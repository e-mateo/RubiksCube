using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float speedMoveCam;
    private Vector3 camToCube;
    private Camera cam;
    private CubeInfo cubeInfo;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        cubeInfo = GetComponent<CubeInfo>();
        camToCube = (cubeInfo.rotatePoint.transform.position - cam.transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseScrool = Input.GetAxis("Mouse ScrollWheel");
        cam.transform.position += mouseScrool * speedMoveCam * Time.deltaTime * camToCube;
    }
}
