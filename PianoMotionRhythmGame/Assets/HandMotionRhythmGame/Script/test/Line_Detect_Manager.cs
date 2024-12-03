using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRPlugin;

public class Line_Detect_Manager : MonoBehaviour
{
    private GameObject centerEye;
    private GameObject leftHand;
    private GameObject rightHand;

    public GameObject Notes;
    public List<GameObject> hit_lines = new List<GameObject>();

    public Vector3 headsetPos;
    public Vector3 lhandPos;
    public Vector3 rhandPos;

    Ray lhand_ray;
    Ray rhand_ray;
    public float rayDistance = 10f;

    void Set_Devices()
    {
        centerEye = GameObject.Find("CenterEyeAnchor");
        leftHand = GameObject.Find("LeftHandAnchor");
        rightHand = GameObject.Find("RightHandAnchor");
    }

    void Set_Hit_Lines()
    {
        foreach (Transform lane in Notes.transform)
        {
            foreach (Transform child in lane)
            {
                if (child.name == "Hit_Line")
                {
                    hit_lines.Add(child.gameObject);
                }
            }
        }
    }

    void Get_Controller_Ray(out Ray lhand_ray, out Ray rhand_ray)
    {
        headsetPos = centerEye.transform.position;
        lhandPos = leftHand.transform.position;
        rhandPos = rightHand.transform.position;
        lhand_ray = new Ray(lhandPos, (lhandPos- headsetPos).normalized);
        rhand_ray = new Ray(rhandPos, (rhandPos - headsetPos).normalized);
    }

    void Check_Note_Collision()
    {
        RaycastHit hit;
        Button3DController hand_bc;

        foreach (GameObject hit_line in hit_lines)
        {
            
            Collider targetCollider = hit_line.GetComponent<BoxCollider>();
            if (targetCollider != null)
            {
                hand_bc = targetCollider.gameObject.GetComponent<Button3DController>();
                if (lhand_ray.direction != Vector3.zero && targetCollider.Raycast(lhand_ray, out hit, rayDistance))
                {
                    hand_bc.islhand = true;
                }
                else
                {
                    hand_bc.islhand = false;
                }


                if (rhand_ray.direction != Vector3.zero && targetCollider.Raycast(rhand_ray, out hit, rayDistance))
                {
                    hand_bc.isrhand = true;
                }
                else
                {
                    hand_bc.isrhand = false;
                }
            }
            else
            {
                Debug.Log("No collidor");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Set_Devices();
        Set_Hit_Lines();
    }

    // Update is called once per frame
    void Update()
    {
        Get_Controller_Ray(out lhand_ray, out rhand_ray);
        Check_Note_Collision();
    }
}
