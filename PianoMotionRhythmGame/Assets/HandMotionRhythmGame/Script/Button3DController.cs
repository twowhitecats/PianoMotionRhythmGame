using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button3DController : MonoBehaviour
{

    private MeshRenderer mr;

    private MeshFilter mf;

    public Material pressedmat;
    public Material defaultmat;

    

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mf = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            mr.material = pressedmat;
        }
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            mr.material = pressedmat;
        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            mr.material = defaultmat;
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            mr.material = defaultmat;
        }

    }
}
