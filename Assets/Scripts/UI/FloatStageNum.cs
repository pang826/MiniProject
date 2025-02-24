using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatStageNum : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] stageTMP;

    private int stageNum;

    private void Start()
    {
        GameManager.Instance.OnChangeState += FloatStageNumber;
        FloatStageNumber();
    }

    private void FloatStageNumber()
    {
        stageNum = GameManager.Instance.Stage;
        StartCoroutine(FloatRoutine());
    }

    IEnumerator FloatRoutine()
    {
        stageTMP[stageNum - 1].gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        stageTMP[stageNum - 1].gameObject.SetActive(false);
    }
}
