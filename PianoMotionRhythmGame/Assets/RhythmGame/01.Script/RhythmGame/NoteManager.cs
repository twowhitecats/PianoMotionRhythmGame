using System.Collections.Generic;
using UnityEngine;
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
            SpawnNote(0);
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

        public void SpawnNote(int index)
        {
            lanes[index].SpawnNote();
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
