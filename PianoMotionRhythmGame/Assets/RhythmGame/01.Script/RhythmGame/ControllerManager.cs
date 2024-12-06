using Oculus.Interaction.Input;
using RhythmGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public bool rbutton1;
    public bool rbutton2;
    public bool lbutton1;
    public bool lbutton2;

    public KeyboardManager keyboardManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void MissVibration(OVRInput.Controller controller)
    {
        VibrateForTime(0.5f, 0.2f, 1.0f, controller);
    }
    public void GreatVibration(OVRInput.Controller controller)
    {
        VibrateForTime(0.2f, 1.0f, 0.3f, controller);
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
            VibrateForTime(0.5f, 0.5f, 1.0f, OVRInput.Controller.LTouch);
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) //R
        {
            keyboardManager.KeyPress(true);
            VibrateForTime(0.5f, 0.5f, 1.0f, OVRInput.Controller.RTouch);
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {

        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {

        }

    }
}
