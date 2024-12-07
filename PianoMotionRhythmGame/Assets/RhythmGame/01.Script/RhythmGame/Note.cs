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
            float elapsedTime = NoteManager.instance.currentTime - spawnTime;
            if(elapsedTime < 0)
            {
                return;
            }
            float y = 480 - (speed * NoteManager.instance.speedMultiplier * elapsedTime);
            this.GetComponent<RectTransform>().anchoredPosition = new Vector2(this.GetComponent<RectTransform>().anchoredPosition.x, y);

            if(this.GetComponent<RectTransform>().anchoredPosition.y <= -540)
            {
                Miss();
            }
        }

        private void Miss()
        {
            Pool.Release(this.gameObject);
            Debug.Log("Miss");
            controllerManager.MissVibration(true);
            controllerManager.MissVibration(false);
            //Remove From notesInLan
        }
    }
}
