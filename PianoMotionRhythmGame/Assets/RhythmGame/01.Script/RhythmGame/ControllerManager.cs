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

        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {

        }

    }
}
