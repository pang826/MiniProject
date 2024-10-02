using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Cloth : MonoBehaviour
{
    [SerializeField] ItemData data; // 이름, 무게, 타입(무기, 치료, 재료)
    [SerializeField] ItemData.Type type;
    public TextMeshProUGUI itemTmp; // UI에 표시할 TMP
    public ScrollRect itemSpotUi;   // ScrollView 위치
    public RectTransform itemSpot;  // ScrollView 컴포넌트의 Content의 위치
    private void Start()
    {
        type = data.type;
        itemSpotUi = GameObject.FindGameObjectWithTag("ItemSpot").GetComponent<ScrollRect>();
        itemSpot = itemSpotUi.content;
    }
    // 근처에 플레이어가 다가왔을 때
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && itemTmp == null)
        {
            itemTmp = itemSpot.AddComponent<TextMeshProUGUI>();
        }
    }
    // 플레이어가 근처에 계속 있을 때
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            itemTmp.text = $"{data.itemName}\t\t{data.Weight.ToString()}\t\t{type.ToString()}";
        }
    }
    // 플레이어가 근처에서 멀어졌을 때
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && itemTmp != null)
        {
            Destroy(itemTmp);
        }
    }
}
