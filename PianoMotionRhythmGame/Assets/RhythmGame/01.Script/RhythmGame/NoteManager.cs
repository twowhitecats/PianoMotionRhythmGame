using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Pool;

namespace RhythmGame
{
    public class NoteManager : MonoBehaviour
    {
        public bool gameStarted;

        [SerializeField] private GameObject obj_note;

        [SerializeField] private List<Lane> lanes = new List<Lane>();
        [SerializeField] private Transform pool;
        [SerializeField] private JSONParser parser;

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

            SetupLanes();
            InitPool();
        }

        private void Start()
        {
            //SpawnNote(0);
        }

        private void Update()
        {
            currentTime = Time.time;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                SpawnNote(0);
            }
            if(gameStarted)
            {
                if(parser.chart.Count != 0 && index < parser.chart.Count && currentTime >= parser.chart[index].targetTime-2f)
                {
                    for (int i = 0; i < parser.chart[index].notes.Count; i++)
                    {
                        SpawnNote(parser.chart[index].notes[i], parser.chart[index].targetTime);
                    }
                    index++;
                }
            }
        }

        private void SetupLanes()
        {
            foreach(Transform obj in transform)
            {
                if(obj.name.Contains("Lane"))
                {
                    lanes.Add(obj.GetComponent<Lane>());
                    obj.GetComponent<Lane>().SetNoteManager(this);
                }
            }
        }

        private void SpawnNote(int laneNum)
        {
            lanes[laneNum].SpawnNote(5f, KeyCode.Space);
        }
        public void SpawnNote(NoteInfo info, float targetTime)
        {
            lanes[info.laneNum].SpawnNote(targetTime, info.button);
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
