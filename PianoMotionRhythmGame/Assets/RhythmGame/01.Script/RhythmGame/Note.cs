using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace RhythmGame
{
    public class Note : MonoBehaviour
    {
        public IObjectPool<GameObject> Pool { get; set; }
        public KeyCode keyToPress;
        public float targetTime = 5;

        [Range(3,12)] public float speedMultiplier = 1f;
        public float speed;
        public float spawnTime;

        public void SetSpawnTime()
        {
            float distance = 920;
            float travelTime = distance / speed;
            float adjustedTravelTime = travelTime / speedMultiplier;
            spawnTime = targetTime - adjustedTravelTime;
        }

        private void Update()
        {
            float elapsedTime = NoteManager.instance.currentTime - spawnTime;
            if(elapsedTime < 0)
            {
                return;
            }
            float y = 480 - (speed * speedMultiplier * elapsedTime);
            this.GetComponent<RectTransform>().anchoredPosition = new Vector2(this.GetComponent<RectTransform>().anchoredPosition.x, y);

            if(this.GetComponent<RectTransform>().anchoredPosition.y <= -440)
            {
                Debug.Log(NoteManager.instance.currentTime);
            }

            if(this.GetComponent<RectTransform>().anchoredPosition.y <= -480)
            {
                Pool.Release(this.gameObject);
            }
        }
    }
}
