using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private NearbyObjectDetector detector;
    public List<PickUpObject> inventory;
    public LinkedList<PickUpObject> Inventory;
    private void Awake()
    {
        detector = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<NearbyObjectDetector>();
    }


}
