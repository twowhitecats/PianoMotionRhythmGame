using RhythmGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    private GameObject centerEye;
    private GameObject leftHand;
    private GameObject rightHand;
    private int blocksize;

    public ControllerManager controllerManager;
    public LaneManager laneManager;

    private MeshRenderer mr;

    public List<GameObject> hit_blocks = new List<GameObject>();
    public List<bool> hit_blocks_lhand_detection = new List<bool>();
    public List<bool> hit_blocks_rhand_detection = new List<bool>();

    public Material defaultmat;
    public Material selectedmat;
    public Material pushedmat;

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
        foreach (Transform block in gameObject.transform)
        {
            hit_blocks.Add(block.gameObject);
            hit_blocks_lhand_detection.Add(false);
            hit_blocks_rhand_detection.Add(false);
        }
    }

    void Get_Controller_Ray(out Ray lhand_ray, out Ray rhand_ray)
    {
        headsetPos = centerEye.transform.position;
        lhandPos = leftHand.transform.position;
        rhandPos = rightHand.transform.position;
        lhand_ray = new Ray(lhandPos, (lhandPos - headsetPos).normalized);
        rhand_ray = new Ray(rhandPos, (rhandPos - headsetPos).normalized);
    }

    void update_material(int ind)
    {

        mr = hit_blocks[ind].GetComponent<MeshRenderer>();

        mr.material = defaultmat;
        if (hit_blocks_lhand_detection[ind] || hit_blocks_rhand_detection[ind])
        {
            mr.material = selectedmat;
            if ((hit_blocks_rhand_detection[ind] && controllerManager.rbutton1) || (hit_blocks_lhand_detection[ind] && controllerManager.lbutton1))
            {
                mr.material = pushedmat;
            }
        }
    }

    void Check_Note_Collision()
    {
        RaycastHit hit;

        for (int i = 0; i < hit_blocks.Count; i++) 
        {

            Collider targetCollider = hit_blocks[i].GetComponent<BoxCollider>();
            if (targetCollider != null)
            {
                if (lhand_ray.direction != Vector3.zero && targetCollider.Raycast(lhand_ray, out hit, rayDistance))
                {
                    hit_blocks_lhand_detection[i] = true;
                }
                else
                {
                    hit_blocks_lhand_detection[i] = false;
                }


                if (rhand_ray.direction != Vector3.zero && targetCollider.Raycast(rhand_ray, out hit, rayDistance))
                {
                    hit_blocks_rhand_detection[i] = true;
                }
                else
                {
                    hit_blocks_rhand_detection[i] = false;
                }
                update_material(i);
            }
            else
            {
                Debug.Log("No collidor");
            }
        }
    }

    public void KeyPress(bool isrhand)
    {
        for (int i = 0; i < hit_blocks.Count; i++)
        {
            if (hit_blocks_rhand_detection[i] && isrhand)
            {
                laneManager.Hit(i);
            }

            if (hit_blocks_lhand_detection[i] && !isrhand)
            {
                laneManager.Hit(i);
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
