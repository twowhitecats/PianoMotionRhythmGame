using FMOD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManager : MonoBehaviour
{
    [Header("Instance")]
    public static AudioManager _instance;

    public AudioManager Instance()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        return _instance;
    }

    [Header("SoundRef")]
    public FMODUnity.EventReference musicEventName; //fmod 이벤트 주소
    public FMODUnity.EventReference sfxEventName;
    FMOD.Studio.EventInstance musicInstance;
    FMOD.Studio.EventInstance sfxInstance;

    [Header("Sound Setting")]
    public float musicVolume = 4.0f;
    public float sfxVolume = 1.0f;
    public int BPM = 146; // 곡 따라 직접 입력 필요.. 본 bpm은 kanon bpm



    int currentTime = 0; //밀리초
    int currentTick = 0;
    int startDelay = 0;
    int ticktime = 0;



#if UNITY_EDITOR
    void Reset()
    {
        musicEventName = FMODUnity.EventReference.Find("event:/music/music");
        sfxEventName = FMODUnity.EventReference.Find("event:/music/music");

    }
#endif

    private void Awake()
    {
        Instance();

        Application.targetFrameRate = 60;

        musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicEventName);
        musicInstance.setVolume(musicVolume);
        sfxInstance = FMODUnity.RuntimeManager.CreateInstance(sfxEventName);
        sfxInstance.setVolume(sfxVolume);
        ticktime = (60 * 1000) / BPM;

        StartMusic();
    }



    // Update is called once per frame
    void Update()
    {

        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger)) 
        {
            if (PlaybackState(musicInstance) != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                StartMusic();
            }
        }

        if (PlaybackState(musicInstance) == FMOD.Studio.PLAYBACK_STATE.PLAYING && currentTime >= ticktime * currentTick)
        {
            currentTick += 1;
            sfxInstance.start();
        }
    }

    private void FixedUpdate()
    {
        if (PlaybackState(musicInstance) == FMOD.Studio.PLAYBACK_STATE.PLAYING) currentTime = GetTime();
    }
    void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicInstance.release();
    }

    public void StartMusic() //음악 시작
    {
        if(PlaybackState(musicInstance) != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            currentTime = 0;
            currentTick = 0;
            musicInstance.start();
            UnityEngine.Debug.Log("Music Starts!");

        }
    }

    public void Resume()
    {
        bool p;
        musicInstance.getPaused(out p);
        if (p == true)
        {
            musicInstance.setPaused(false);
            UnityEngine.Debug.Log("Resume!");
        }
    }

    public void PauseMusic()
    {
        if (PlaybackState(musicInstance) == FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            musicInstance.setPaused(true);

            currentTime = GetTime();
            UnityEngine.Debug.Log("Music Paused!");
        }
    }

    public void StopMusic()
    {
        if (PlaybackState(musicInstance) != FMOD.Studio.PLAYBACK_STATE.STOPPED)
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            UnityEngine.Debug.Log("Music Stopped!");
        }
    }

    FMOD.Studio.PLAYBACK_STATE PlaybackState(FMOD.Studio.EventInstance instance) //음악 재생상태 enum값으로 반환 https://www.fmod.com/docs/2.02/api/studio-api-common.html#fmod_studio_playback_state
    {
        FMOD.Studio.PLAYBACK_STATE pS;
        instance.getPlaybackState(out pS);
        return pS;
    }

    public int GetTime() //bgm 밀리초 반환
    {
        musicInstance.getTimelinePosition(out int position);
        return position;
    }
}
