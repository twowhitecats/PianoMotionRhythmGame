using Oculus.Interaction.Input;
using RhythmGame;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public Transform controllerParent;

    public bool rbutton1;
    public bool rbutton2;
    public bool lbutton1;
    public bool lbutton2;

    public Vector3 rwrist;
    public Quaternion rwrist_rot;
    public Vector3 rwrist_forward;
    public Vector3 rwrist_up;
    public Vector3 lwrist;
    public Quaternion lwrist_rot;
    public Vector3 lwrist_forward;
    public Vector3 lwrist_up;

    private LineRenderer forward_L;
    private LineRenderer up_L;
    private LineRenderer forward_R;
    private LineRenderer up_R;

    public KeyboardManager keyboardManager;
    public MusicController musicController;
    public AnimationManager animationManager;

    // Start is called before the first frame update
    void Start()
    {
        forward_L= GameObject.Find("forward_L").GetComponent<LineRenderer>();
        forward_R = GameObject.Find("forward_R").GetComponent<LineRenderer>();
        up_L = GameObject.Find("up_L").GetComponent<LineRenderer>();
        up_R = GameObject.Find("up_R").GetComponent<LineRenderer>();
    }
    public void MissVibration(bool isrhand)
    {
        if (isrhand)
        {
            VibrateForTime(0.5f, 0.2f, 1.0f, OVRInput.Controller.RTouch);
        }
        else
        {
            VibrateForTime(0.5f, 0.2f, 1.0f, OVRInput.Controller.LTouch);
        }
    }
    public void GreatVibration(bool isrhand)
    {
        if (isrhand)
        {
            VibrateForTime(0.2f, 1.0f, 0.3f, OVRInput.Controller.LTouch);
        }
        else
        {
            VibrateForTime(0.2f, 1.0f, 0.3f, OVRInput.Controller.LTouch);
        }
    }

    private void VibrateForTime(float duration, float frequency, float amplitude, OVRInput.Controller controller)
    {
        StartCoroutine(VibrationCoroutine(duration, frequency, amplitude, controller));
    }

    private IEnumerator VibrationCoroutine(float duration, float frequency, float amplitude, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);

        yield return new WaitForSeconds(duration);

        OVRInput.SetControllerVibration(0, 0, controller);
    }

    void Update()
    {
        lbutton1 = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
        rbutton1 = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
        lbutton2 = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger);
        rbutton2 = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) //L
        {
            keyboardManager.KeyPress(false);
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) //R
        {
            keyboardManager.KeyPress(true);

        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            musicController.PlayPauseMusic();
            animationManager.animation_start();
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            musicController.StopMusic();
            animationManager.animation_stop();
        }

        rwrist = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        rwrist = controllerParent.TransformPoint(rwrist);
        rwrist_rot=OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        rwrist_rot = controllerParent.rotation * rwrist_rot;
        rwrist_forward = rwrist_rot * Vector3.forward;
        rwrist_up = rwrist_rot * Vector3.right;

        lwrist = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        lwrist = controllerParent.TransformPoint(lwrist);
        lwrist_rot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
        lwrist_rot = controllerParent.rotation * lwrist_rot;
        lwrist_forward = lwrist_rot * Vector3.forward;
        lwrist_up = lwrist_rot * -Vector3.right;

        forward_L.SetPosition(0, lwrist);
        forward_L.SetPosition(1, lwrist + lwrist_forward * 0.1f);
        up_L.SetPosition(0, lwrist);
        up_L.SetPosition(1, lwrist + lwrist_up * 0.1f);
        forward_R.SetPosition(0, rwrist);
        forward_R.SetPosition(1, rwrist + rwrist_forward * 0.1f);
        up_R.SetPosition(0, rwrist);
        up_R.SetPosition(1, rwrist + rwrist_up*0.1f);
    }
}
