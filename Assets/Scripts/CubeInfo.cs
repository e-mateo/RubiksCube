using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class CubeInfo : MonoBehaviour
{
    [HideInInspector] public bool isRotating;
    [HideInInspector] public bool pause;
    [HideInInspector] public bool onShuffle;
    private bool onControls;
    [HideInInspector] public int sizeCube;
    [HideInInspector] public int shuffleSize;

    public float speedRotateFace;
    public float speedRotateCube;

    [HideInInspector] public Vector3 normalUp;
    [HideInInspector] public Vector3 normalRight;
    [HideInInspector] public GameObject rotatePoint;
    [HideInInspector] public GameObject Rubik;

    [SerializeField] private Slider sliderSize;
    [SerializeField] private Slider sliderMix;
    [SerializeField] private TextMeshProUGUI textSize;
    [SerializeField] private TextMeshProUGUI textMix;
    [SerializeField] private TextMeshProUGUI solved;
    [SerializeField] private TextMeshProUGUI controls;

    [SerializeField] private GameObject ButtonChange;
    [SerializeField] private GameObject ButtonReturn;
    [SerializeField] private GameObject ButtonControls;
    private GenerateCube generate;
    private Shuffle shuffle;


    void Start()
    {
        sizeCube = 3;
        shuffleSize = 10;
        isRotating = false;
        pause = false;
        onShuffle = false;
        generate = GetComponent<GenerateCube>();
        shuffle = GetComponent<Shuffle>();
    }

    private void Update()
    {
        if(pause)
        {
            sizeCube = (int)sliderSize.value;
            shuffleSize = (int)sliderMix.value;

            textSize.SetText("Cube Size : " + sizeCube);
            textMix.SetText("Shuffle Size : " + shuffleSize);
        }
    }

    public void OpenOptions()
    {
        pause = true;
        ButtonChange.SetActive(false);
        ButtonControls.SetActive(false);
        ButtonReturn.SetActive(true);
        sliderSize.gameObject.SetActive(true);
        sliderMix.gameObject.SetActive(true);
        Rubik.SetActive(false);
        solved.gameObject.SetActive(false);

        generate.DestroyCube();
        shuffle.StopCoroutines();
    }

    public void CloseOptions()
    {
        ButtonChange.SetActive(true);
        ButtonControls.SetActive(true);
        ButtonReturn.SetActive(false);
        Rubik.SetActive(true);
        sliderSize.gameObject.SetActive(false);
        sliderMix.gameObject.SetActive(false);

        pause = false;
        onShuffle = false;
        isRotating = false;
    }

    public void Return()
    {
        if (pause)
            CloseOptions();
        else if(onControls)
            CloseControls();
    }

    public void OpenControls()
    {
        onControls = true;
        ButtonChange.SetActive(false);
        ButtonControls.SetActive(false);
        controls.gameObject.SetActive(true);
        Rubik.SetActive(false);
        ButtonReturn.SetActive(true);
    }

    public void CloseControls()
    {
        onControls = false;
        ButtonChange.SetActive(true);
        ButtonControls.SetActive(true);
        controls.gameObject.SetActive(false);
        Rubik.SetActive(true);
        ButtonReturn.SetActive(false);
    }

    public void isSolved()
    {
        Quaternion rotationToCheck = Quaternion.identity;
        bool first = true;

        foreach(Transform cubes in Rubik.transform)
        {
            if(cubes != Rubik.transform && cubes != rotatePoint.transform)
            {
                if(first)
                {
                    rotationToCheck = cubes.rotation;
                    first = false;
                }
                else if(!isQuaternionsEquals(rotationToCheck, cubes.rotation, 0.01f))
                {
                    solved.gameObject.SetActive(false);
                    return;
                }
            }
        }
        solved.gameObject.SetActive(true);
    }

    public bool isQuaternionsEquals(Quaternion quatA, Quaternion quatB, float precision)
    {
        return Mathf.Abs(Quaternion.Dot(quatA, quatB)) >= 1 - precision;
    }

    public void Quit()
    {
        Application.Quit();
    }

}