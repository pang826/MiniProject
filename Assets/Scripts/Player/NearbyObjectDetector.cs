using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearbyObjectDetector : MonoBehaviour
{
    [SerializeField] private List<PickUpObject> objects;

    [SerializeField] private GameObject itemSpot;

    [SerializeField] private GameObject prefab;

    private void Awake()
    {
        itemSpot = GameObject.FindGameObjectWithTag("ItemSpot").transform.GetChild(0).GetChild(0).gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PickUpObject obj))
        {
            obj.FloatItemInformation(itemSpot, prefab, obj, objects);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PickUpObject obj))
        {
            obj.DisableItemInformation(obj, objects);
        }
    }
}