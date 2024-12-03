using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{

    private MeshRenderer mr;
    private MeshFilter mf;

    public Material missmat;
    public Material perfectmat;
    public Material defaultmat;

    public Line_Manager line_manager;

    private Coroutine currentCoroutine;

    private void Move()
    {
        gameObject.transform.position = gameObject.transform.position - new Vector3(0.0f, 0.001f, 0.0f);
    }

    public void Init()
    {
        gameObject.SetActive(true);
        gameObject.transform.position = line_manager.spawn_line.transform.position;
        mr.material = defaultmat;
    }

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mf = GetComponent<MeshFilter>();
        currentCoroutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            Move();
        }

        if (gameObject.transform.position.y < line_manager.judgment_line.transform.position.y)
        {
            Timing_Judgment();
        }
    }
    IEnumerator DisableWithPooling()
    {
        if (line_manager.judgment_line.transform.position.y < gameObject.transform.position.y && gameObject.transform.position.y < line_manager.judgment_line.transform.position.y+0.03f)
        {
            mr.material = perfectmat;
            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(false);
        }
        else
        {
            mr.material = missmat;
            yield return new WaitForSeconds(0.2f);
            mr.material = perfectmat;
            gameObject.SetActive(false);
        }
        currentCoroutine = null;
    }

    public void Timing_Judgment()
    {
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(DisableWithPooling());
        }
    }
}
