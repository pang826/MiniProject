using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject man;
    [SerializeField] GameObject woman;

    [SerializeField] Transform[] spawnPoints = new Transform[4];
    private Vector3[] spawnPoint = new Vector3[4];

    private int stage;

    private void Start()
    {
        spawnPoint[0] = spawnPoints[0].position;
        spawnPoint[1] = spawnPoints[1].position;
        spawnPoint[2] = spawnPoints[2].position;
        spawnPoint[3] = spawnPoints[3].position;
        GameManager.Instance.OnChangeState += Spawn;
        stage = GameManager.Instance.Stage;
        Spawn();
    }

    private void Update()
    {
    }

    private void Spawn()
    {
        StartCoroutine(SpawnRoutine());
    }
    

    IEnumerator SpawnRoutine()
    {
        float curTime = 0f;
        int repeatCount = GameManager.Instance.Cycle;
        int curRepeatCount = 0;
        while(true)
        {
            curTime += Time.deltaTime;
            if(curTime >= GameManager.Instance.Interval && curRepeatCount < repeatCount)
            {
                curTime = 0f;
                curRepeatCount++;
                GameObject.Instantiate(woman, spawnPoint[0], Quaternion.identity);
                GameObject.Instantiate(woman, spawnPoint[1], Quaternion.identity);
                GameObject.Instantiate(woman, spawnPoint[2], Quaternion.identity);
                GameObject.Instantiate(woman, spawnPoint[3], Quaternion.identity);
            }
            else if(curRepeatCount >= repeatCount) { yield break; }
            yield return null;
        }
        
    }
}
