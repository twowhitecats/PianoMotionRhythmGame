using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteLine : MonoBehaviour
{
    private bool lineLenInitialized = false;
    [SerializeField]
    private GameObject notePrefab;

    [SerializeField]
    private int maxNoteInLine = 10;

    private GameObject[] _notes;

    // Start is called before the first frame update
    private void Awake()
    {
        _notes = new GameObject[maxNoteInLine];
        for (int i = 0; i < maxNoteInLine; i++) {
            GameObject note = Instantiate(notePrefab, this.transform);
            _notes[i] = note;
        }
        
    }

    void Start()
    {
        StartCoroutine(LineStretch());

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LineStretch()
    {
        Vector3 linescale = new Vector3(1.0f, 1.0f, 10.0f);
        Vector3 lerpvec = new Vector3(1f, 1f, 1f);
        while (transform.localScale.z < 9.9f)
        {
            Debug.Log("localvec" + transform.localScale);

            float lerpz = Mathf.Lerp(transform.localScale.z, linescale.z,  0.4f);
            lerpvec = new Vector3(1, 1, lerpz);
            lerpvec -= transform.localScale;

            Debug.Log("lerpvec" + lerpvec);

            transform.localScale += lerpvec;
            yield return new WaitForSeconds(0.05f);
        }
        transform.localScale = linescale;
        lineLenInitialized = true;
        yield break;
    }

    private void LineInitializer()
    {

    }
}
