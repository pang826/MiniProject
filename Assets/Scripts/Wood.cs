using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.VisualScripting;
public class Wood : MonoBehaviour
{
    [SerializeField] ItemData data; // 이름, 무게, 타입(무기, 치료, 재료)
    [SerializeField] ItemData.Type type;
    public TextMeshProUGUI itemTmp;
    public ScrollRect itemSpotUi;
    public RectTransform itemSpot;
    private void Start()
    {
        type = data.type;
        itemSpotUi = GameObject.FindGameObjectWithTag("ItemSpot").GetComponent<ScrollRect>();
        itemSpot = itemSpotUi.content;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            itemTmp = itemSpot.AddComponent<TextMeshProUGUI>();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            itemTmp.text = $"{data.itemName}\t\t{data.Weight.ToString()}\t\t{type.ToString()}";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(itemTmp);
        }
    }
}
