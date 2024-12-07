using Meta.XR.MultiplayerBlocks.Fusion.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

namespace RhythmGame
{
    public class Note : MonoBehaviour
    {
        public IObjectPool<GameObject> Pool { get; set; }
        public KeyCode keyToPress;
        public float targetTime = 5;

        public float speed;
        public float spawnTime;

        private float elapsedTime;

        private float y;
        private ControllerManager controllerManager;
        public void Start()
        {
            controllerManager=GameObject.Find("ControllerManager").GetComponent<ControllerManager>();
        }

        public void SetSpawnTime()
        {
            float distance = 920;
            float travelTime = distance / speed;
            float adjustedTravelTime = travelTime / NoteManager.instance.speedMultiplier;
            spawnTime = targetTime - adjustedTravelTime;
        }

        private void Update()
        {
            elapsedTime = NoteManager.instance.currentTime - spawnTime;
            if(NoteManager.instance.currentMode == Mode.Game || NoteManager.instance.currentMode == Mode.Test)
            {
                if(elapsedTime < 0)
                {
                    return;
                }

                Move();

                if(CheckEnd())
                {
                    speed = 0;
                    Miss();
                }
            }
            else if(NoteManager.instance.currentMode == Mode.Editing)
            {
                if(elapsedTime < 0)
                {
                    return;
                }

                SetSpawnTime();
                Move();

                if(CheckEnd())
                {
                    Pool.Release(this.gameObject);
                }
            }
        }
        private bool CheckEnd()
        {
            return this.GetComponent<RectTransform>().anchoredPosition.y <= -500;
        }

        private void Move()
        {
            y = 480 - (speed * NoteManager.instance.speedMultiplier * elapsedTime);
            this.GetComponent<RectTransform>().anchoredPosition = new Vector2(this.GetComponent<RectTransform>().anchoredPosition.x, y);
        }

        private void Miss()
        {
            Pool.Release(this.gameObject);
            Debug.Log("Miss");
            //Remove From notesInLane
            ScoreManager._instance.NoteMiss();
        }
        public void Release()
        {
            Pool.Release(this.gameObject);
            controllerManager.MissVibration(true);
            controllerManager.MissVibration(false);
            //Remove From notesInLane
        }
        public void OnEdit()
        {
            NoteEditor noteEditor = GameObject.Find("NoteEditor").GetComponent<NoteEditor>();
            noteEditor.SetNoteToEdit(this);
        }
    }
}
