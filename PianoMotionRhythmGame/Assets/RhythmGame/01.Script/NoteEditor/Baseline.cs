using UnityEngine;
using UnityEngine.Pool;

namespace RhythmGame
{
    public class Baseline : MonoBehaviour
    {
        public IObjectPool<GameObject> Pool { get; set; }

        public float targetTime;

        public float speed;
        public float spawnTime;
        private float elapsedTime;

        private float y;

        public void SetSpawnTime()
        {
            float distance = 920;
            float travelTime = distance / speed;
            float adustedTravelTime = travelTime / NoteManager.instance.speedMultiplier;
            spawnTime = targetTime - adustedTravelTime;
        }
        void Start()
        {

        }

        void Update()
        {
            elapsedTime = NoteManager.instance.currentTime - spawnTime;
            if(NoteManager.instance.currentMode == Mode.Editing)
            {
                if(elapsedTime < 0)
                {
                    return;
                }

                SetSpawnTime();
                Move();

                if(CheckEnd())
                {
                    Pool.Release(gameObject);
                }
            }
        }

        private void Move()
        {
            y = 480 - (speed * NoteManager.instance.speedMultiplier * elapsedTime);
            this.GetComponent<RectTransform>().anchoredPosition = new Vector2(this.GetComponent<RectTransform>().anchoredPosition.x, y);
        }
        private bool CheckEnd()
        {
            return this.GetComponent<RectTransform>().anchoredPosition.y <= -500;
        }
    }
}
