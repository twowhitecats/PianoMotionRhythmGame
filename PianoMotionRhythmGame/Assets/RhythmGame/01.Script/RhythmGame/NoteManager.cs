using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Pool;

namespace RhythmGame
{
    public class NoteManager : MonoBehaviour
    {
        public bool gameStarted;
        [SerializeField] private bool testing;

        [SerializeField] private GameObject obj_note;

        [SerializeField] private Transform pool;
        [SerializeField] private JSONParser parser;
        [SerializeField] private LaneManager laneManager;

        public IObjectPool<GameObject> NotePool { get; private set; }
        [SerializeField] private int defaultPoolSize;
        [SerializeField] private int maxPoolSize;

        private int index = 0;
        public float currentTime;
        [Range(3,12)] public float speedMultiplier = 6.0f;

        public static NoteManager instance;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            InitPool();
        }

        private void Start()
        {
            //SpawnNote(0);
        }

        private void Update()
        {
            //currentTime = Time.time;
            currentTime = AudioManager._instance.GetTime()/1000f;
            if(testing)
            {
                if(Input.GetKeyDown(KeyCode.Alpha1))
                {
                    laneManager.SpawnNote(0, currentTime + 2f);
                }
                if(Input.GetKeyDown(KeyCode.Alpha2))
                {
                    laneManager.SpawnNote(1, currentTime + 2f);
                }
                if(Input.GetKeyDown(KeyCode.Alpha3))
                {
                    laneManager.SpawnNote(2, currentTime + 2f);
                }
                if(Input.GetKeyDown(KeyCode.Alpha4))
                {
                    laneManager.SpawnNote(3, currentTime + 2f);
                }
                if(Input.GetKeyDown(KeyCode.Alpha5))
                {
                    laneManager.SpawnNote(4, currentTime + 2f);
                }
                if(Input.GetKeyDown(KeyCode.Alpha6))
                {
                    laneManager.SpawnNote(5, currentTime + 2f);
                }
            }
            if(gameStarted)
            {
                if(parser.chart.Count != 0 && index < parser.chart.Count && currentTime >= parser.chart[index].targetTime-2f)
                {
                    for (int i = 0; i < parser.chart[index].notes.Count; i++)
                    {
                        laneManager.SpawnNote(parser.chart[index].notes[i], parser.chart[index].targetTime);
                    }
                    index++;
                }
            }
        }

        private void InitPool()
        {
            NotePool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject,
                true, defaultPoolSize, maxPoolSize);

            for(int i = 0; i < defaultPoolSize; i++)
            {
                Note note = CreatePooledItem().GetComponent<Note>();
                note.Pool.Release(note.gameObject);
            }
        }

        private GameObject CreatePooledItem()
        {
            GameObject item = Instantiate(obj_note);
            item.GetComponent<Note>().Pool = this.NotePool;
            item.transform.SetParent(pool.transform, false);
            return item;
        }

        private void OnTakeFromPool(GameObject item)
        {
            item.SetActive(true);
        }
        private void OnReturnedToPool(GameObject item)
        {
            item.SetActive(false);
        }

        private void OnDestroyPoolObject(GameObject item)
        {
            Destroy(item);
        }
    }
}
