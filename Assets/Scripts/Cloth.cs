using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Cloth : MonoBehaviour
{
    [SerializeField] ItemData data; // �̸�, ����, Ÿ��(����, ġ��, ���)
    [SerializeField] ItemData.Type type;
    public TextMeshProUGUI itemTmp; // UI�� ǥ���� TMP
    public ScrollRect itemSpotUi;   // ScrollView ��ġ
    public RectTransform itemSpot;  // ScrollView ������Ʈ�� Content�� ��ġ
    private void Start()
    {
        type = data.type;
        itemSpotUi = GameObject.FindGameObjectWithTag("ItemSpot").GetComponent<ScrollRect>();
        itemSpot = itemSpotUi.content;
    }
    // ��ó�� �÷��̾ �ٰ����� ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && itemTmp == null)
        {
            itemTmp = itemSpot.AddComponent<TextMeshProUGUI>();
        }
    }
    // �÷��̾ ��ó�� ��� ���� ��
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            itemTmp.text = $"{data.itemName}\t\t{data.Weight.ToString()}\t\t{type.ToString()}";
        }
    }
    // �÷��̾ ��ó���� �־����� ��
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && itemTmp != null)
        {
            Destroy(itemTmp);
        }
    }
}
