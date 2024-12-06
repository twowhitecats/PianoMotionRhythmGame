using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace RhythmGame
{
    public class BaselineManager : MonoBehaviour
    {
        [SerializeField] private GameObject obj_baseline;
        [SerializeField] private int defaultPoolSize;
        [SerializeField] private int maxPoolSize;
        public List<GameObject> activeInPool = new List<GameObject>();
        public IObjectPool<GameObject> LinePool { get; private set; }

        private float index;
        [SerializeField] private int beat;

        private void Awake()
        {
            InitPool();
        }

        void Update()
        {
            //DespawnBaseline();
            if (NoteManager.instance.currentTime != 0 && NoteManager.instance.currentTime >= index - 2f)
            {
                SpawnBaseline();
                index += 1f / beat;
            }
        }

        private void SpawnBaseline()
        {
            var go = LinePool.Get();
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(this.GetComponent<RectTransform>().anchoredPosition.x, 480);
            go.GetComponent<Baseline>().targetTime = index;
            go.GetComponent<Baseline>().SetSpawnTime();
        }
        private void DespawnBaseline()
        {
            for (int i = activeInPool.Count - 1; i >= 0; i--)
            {
                GameObject baseline = activeInPool[i];
                float baselineTime = baseline.GetComponent<Baseline>().targetTime;

                if (NoteManager.instance.currentTime <= baselineTime - 2)
                {
                    LinePool.Release(baseline);
                }
            }
        }
        private void InitPool()
        {
            LinePool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject,
                true, defaultPoolSize, maxPoolSize);

            for (int i = 0; i < defaultPoolSize; i++)
            {
                Baseline baseline = CreatePooledItem().GetComponent<Baseline>();
                baseline.Pool.Release(baseline.gameObject);
            }
        }

        private GameObject CreatePooledItem()
        {
            GameObject item = Instantiate(obj_baseline);
            item.GetComponent<Baseline>().Pool = this.LinePool;
            item.transform.SetParent(transform, false);
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

        public void ResetLines()
        {
            for(int i = 0; i < activeInPool.Count; i++)
            {
                LinePool.Release(activeInPool[i]);
            }
            index = 0;
        }
    }
}
