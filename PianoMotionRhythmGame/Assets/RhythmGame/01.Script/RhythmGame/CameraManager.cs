using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera cam;
    public float ratio=0.9f;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("CenterEyeAnchor").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Matrix4x4 proj = cam.projectionMatrix;

        proj[2, 2] *= ratio; // 깊이값을 줄여 원근감 증가
        proj[2, 3] *= ratio;

        cam.projectionMatrix = proj;
    }
}
