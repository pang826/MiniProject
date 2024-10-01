using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Wood : MonoBehaviour
{
    [SerializeField] ItemData data; // �̸�, ����, Ÿ��(����, ġ��, ���)
    [SerializeField] ItemData.Type type;
    public TextMeshProUGUI nameTmp;
    public TextMeshProUGUI weightTmp;
    public TextMeshProUGUI typeTmp;
    private void Start()
    {
        type = data.type;
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            nameTmp.text = data.itemName;
            weightTmp.text = data.Weight.ToString();
            typeTmp.text = this.type.ToString();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            nameTmp.text = null;
            weightTmp.text = null;
            typeTmp.text = null;
        }
    }
}
