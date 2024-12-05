using OVR.OpenVR;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Pool;

namespace RhythmGame
{
    public class NoteManager : MonoBehaviour
    {
        public Mode currentMode;
        public bool gameStart;

        [SerializeField] private GameObject obj_note;

        [SerializeField] private Transform pool;
        private List<GameObject> activeInPool = new List<GameObject>();

        [SerializeField] private JSONParser parser;
        [SerializeField] private LaneManager laneManager;
        [SerializeField] private MusicController MusicController;

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
            if(currentMode == Mode.Test)
            {
                SpawnNoteByNumberKey();
            }
            else if(currentMode == Mode.Editing)
            {
                currentTime = MusicController.GetCurrentTime()/1000f;
            }
            else if(currentMode == Mode.Game && gameStart)
            {
                currentTime = AudioManager._instance.GetTime()/1000f;
                SpawnNoteByJSON();
            }
        }

        private void SpawnNoteByNumberKey()
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

        private void SpawnNoteByJSON()
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
            activeInPool.Add(item);
        }
        private void OnReturnedToPool(GameObject item)
        {
            item.SetActive(false);
            activeInPool.Remove(item);
        }

        private void OnDestroyPoolObject(GameObject item)
        {
            Destroy(item);
            activeInPool.Remove(item);
        }

        public void ResetIndex() => index = 0;
    }
}

public enum Mode
{
    Game = 0,
    Editing = 1,
    Test = 2
}
