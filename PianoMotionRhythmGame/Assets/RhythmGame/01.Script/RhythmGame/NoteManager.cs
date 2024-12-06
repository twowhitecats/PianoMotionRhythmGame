using OVR.OpenVR;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Pool;

namespace RhythmGame
{
    public class NoteManager : MonoBehaviour
    {
        public static NoteManager instance;

        public Mode currentMode;
        public bool gameStart;
        public float currentTime;

        [Header("Prefab")]
        [SerializeField] private GameObject obj_note;

        [Header("References")]
        [SerializeField] private Transform notePool;
        [SerializeField] private JSONParser parser;
        [SerializeField] private LaneManager laneManager;
        [SerializeField] private MusicController MusicController;

        [Header("ObjectPooling")]
        [SerializeField] private int defaultPoolSize;
        [SerializeField] private int maxPoolSize;
        public List<GameObject> activeInPool = new List<GameObject>();
        public IObjectPool<GameObject> NotePool { get; private set; }

        [Header("Editing")]
        [SerializeField] private float spawnOffset;
        [SerializeField] private float despawnOffset;

        [Header("Setting")]
        [Range(3,12)] public float speedMultiplier = 6.0f;

        private int index = 0;

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
                ManageNoteByTime();
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
        private void ManageNoteByTime()
        {
            foreach(NoteTiming noteData in parser.chart)
            {
                if(Mathf.Abs(noteData.targetTime - currentTime) <= spawnOffset)
                {
                    SpawnNoteByTime(noteData);
                }
            }

            for (int i = activeInPool.Count - 1; i >= 0; i--)
            {
                GameObject note = activeInPool[i];
                float noteTime = note.GetComponent<Note>().targetTime;

                if (currentTime <= noteTime - despawnOffset)
                {
                    NotePool.Release(note);
                }
            }
        }
        private void SpawnNoteByTime(NoteTiming noteData)
        {
            for (int i = 0; i < noteData.notes.Count; i++)
            {
                if (activeInPool.Exists(n => n.GetComponent<Note>().targetTime == noteData.targetTime))
                    continue;

                laneManager.SpawnNote(noteData.notes[i], noteData.targetTime);
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
            item.transform.SetParent(notePool, false);
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
        public int GetIndex() => index;
    }
}

public enum Mode
{
    Game = 0,
    Editing = 1,
    Test = 2
}
