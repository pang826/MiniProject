using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStageData", menuName = "Wave System/Stage Data")]
[System.Serializable]
public class StageData : ScriptableObject
{
    [Tooltip("총 좀비수")] public int ZombieAmount;
    [Tooltip("소환 사이클")] public int SpawnCycle;
    [Tooltip("1회 소환 시 소환되는 양")] public int SpawnPerCount;
    [Tooltip("소환 시간 간격")] public float SpawnInterval;
}
