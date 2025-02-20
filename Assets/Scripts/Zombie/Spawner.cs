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

    private bool isSpawn;

    private void Start()
    {
        spawnPoint[0] = spawnPoints[0].position;
        spawnPoint[1] = spawnPoints[1].position;
        spawnPoint[2] = spawnPoints[2].position;
        spawnPoint[3] = spawnPoints[3].position;
        GameManager.Instance.OnChangeState += Spawn;
        stage = GameManager.Instance.Stage;
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == stage && isSpawn == false)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        StartCoroutine(SpawnRoutine());
    }
    IEnumerator SpawnRoutine()
    {
        if (isSpawn == true)
            yield break;
        isSpawn = true;
        float curTime = 0f;
        while (true)
        {
            curTime += Time.deltaTime;
            if (curTime >= 3f)
            {
                GameObject.Instantiate(man, spawnPoint[0], Quaternion.identity);
                GameObject.Instantiate(woman, spawnPoint[1], Quaternion.identity);
                curTime = 0f;
                yield break;
            }
            yield return null;
        }
    }
}
