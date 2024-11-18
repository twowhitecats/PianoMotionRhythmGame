using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button3DController : MonoBehaviour
{

    private MeshRenderer mr;
    private MeshFilter mf;

    public Line_Manager line_manager;

    public Material pressedmat;
    public Material selectedmat;
    public Material defaultmat;

    public bool islhand;
    public bool isrhand;



    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mf = GetComponent<MeshFilter>();
        islhand = false;
        islhand = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (islhand)
        {
            mr.material = selectedmat;
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                mr.material = pressedmat;
            }

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                line_manager.Get_Note().GetComponent<Note>().Timing_Judgment();
            }
        }
        else if(isrhand)
        {
            mr.material = selectedmat;
            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                mr.material = pressedmat;
            }

            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                line_manager.Get_Note().GetComponent<Note>().Timing_Judgment();
            }
        }
        else
        {
            mr.material = defaultmat;
        }

    }
}
