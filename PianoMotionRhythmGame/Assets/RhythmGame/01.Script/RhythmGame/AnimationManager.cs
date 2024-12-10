using RhythmGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AnimationManager : MonoBehaviour
{

    public Transform LrootBone;
    public Transform RrootBone;
    public ControllerManager controllerManager;
    public MusicController musicController;

    private Vector3 fixedPosition;
    private GameObject leftHand;
    private GameObject rightHand;

    public Animator L_animator;
    public Animator R_animator;
    public bool direction_modify;

    private Quaternion rwrist_rot_offset;
    private Quaternion lwrist_rot_offset;

    void Start()
    {
        leftHand = GameObject.Find("LeftHandAnchor");
        rightHand = GameObject.Find("RightHandAnchor");
        rwrist_rot_offset= Quaternion.Euler(0, 90, 0);
        lwrist_rot_offset = Quaternion.Euler(0, -90, 0);
        L_animator=LrootBone.parent.gameObject.GetComponent<Animator>();
        R_animator = RrootBone.parent.gameObject.GetComponent<Animator>();
        
    }

    public void animation_start()
    {
        L_animator.SetBool("playing",true);
        R_animator.SetBool("playing", true);
        direction_modify = false;
    }
    public void animation_stop()
    {
        L_animator.SetBool("playing", false);
        R_animator.SetBool("playing", false);
        direction_modify = true;
    }

    void LateUpdate()
    {
        // Root Bone의 위치를 고정
        if (LrootBone != null)
        {
            //LrootBone.position = leftHand.transform.position;
            //RrootBone.position = rightHand.transform.position;
            LrootBone.position = controllerManager.lwrist;

            RrootBone.position = controllerManager.rwrist;
            if (direction_modify)
            {
                LrootBone.rotation = Quaternion.LookRotation(-Vector3.Cross(controllerManager.lwrist_up, controllerManager.lwrist_forward).normalized, controllerManager.lwrist_up);
                RrootBone.rotation = Quaternion.LookRotation(-controllerManager.rwrist_up, Vector3.Cross(controllerManager.rwrist_up, controllerManager.rwrist_forward).normalized);
            }
        }
    }
}
