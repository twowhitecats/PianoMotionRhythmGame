using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRPlugin;

public class test_notetouch : MonoBehaviour
{

    private MeshRenderer mr;
    private MeshFilter mf;

    public Material pressedmat;
    public Material defaultmat;

    GameObject centerEye;
    GameObject leftHand;
    GameObject rightHand;
    public Vector3 headsetPos;
    public Vector3 lhandPos;
    public Vector3 rhandPos;

    void Start()
    {
        centerEye = GameObject.Find("CenterEyeAnchor");
        leftHand = GameObject.Find("LeftHandAnchor");
        rightHand = GameObject.Find("RightHandAnchor");
    }

    void Update()
    {
        headsetPos = centerEye.transform.position;
        lhandPos = leftHand.transform.position;
        rhandPos = rightHand.transform.position;

        gameObject.transform.position = lhandPos;
    }
}
