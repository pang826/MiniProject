using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [SerializeField] private StageData[] stageData;
    private StageData curStageData;

    private int stage = 1;
    
    public int Stage { get { return stage; } }

    private int amount;
    
    public int Amount { get { return amount; } }

    private int cycle;
    
    public int Cycle { get { return cycle; } }

    private int perCount;
    public int PerCount { get { return perCount; } }

    private float interval;
    public float Interval { get { return interval; } }

    private int curStageZomCount;

    public int CurStageZomCount { get { return curStageZomCount; }  set { curStageZomCount = value; } }

    public UnityAction OnChangeState;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        curStageData = stageData[stage - 1];
    }

    private void Start()
    {
        OnChangeState += StageUp;
        amount = curStageData.ZombieAmount;
        cycle = curStageData.SpawnCycle;
        perCount = curStageData.SpawnPerCount;
        interval = curStageData.SpawnInterval;
        curStageZomCount = curStageData.ZombieAmount;
    }

    private void StageUp()
    {
        stage++;
        curStageData = stageData[(stage - 1)];
        amount = curStageData.ZombieAmount;
        cycle = curStageData.SpawnCycle;
        perCount = curStageData.SpawnPerCount;
        interval = curStageData.SpawnInterval;
        curStageZomCount = curStageData.ZombieAmount;
    }

    public void DeadZombie()
    {
        curStageZomCount--;
        if (curStageZomCount == 0)
            OnChangeState?.Invoke();
    }
}
