using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScroller : MonoBehaviour
{

    public float beatTempo = 180.0f;

    public bool hasStarted;
    // Start is called before the first frame update
    void Start()
    {
        beatTempo = beatTempo / 60.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            if (OVRInput.GetDown(OVRInput.Button.Any))
            {
                hasStarted = true;
            }
        }
        else
        {
            transform.position -= new Vector3(0f, 0f, beatTempo * Time.deltaTime);
        }


    }
}
