using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static  GameManager Instance { get { return instance; } }
    [SerializeField] TextMeshProUGUI scoreBoard;
    float time = 0;
    [SerializeField] int score;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        DontDestroyOnLoad(gameObject);
    }
    
    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "GameScene")
        {
            time += Time.deltaTime;
            score = (int)time;
            scoreBoard.text = score.ToString();
        }
    }
}
