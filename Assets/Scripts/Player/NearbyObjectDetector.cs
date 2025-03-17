using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearbyObjectDetector : MonoBehaviour
{
    [SerializeField] private RectTransform itemSpot;

    private void Awake()
    {
        itemSpot = GameObject.FindGameObjectWithTag("ItemSpot").GetComponent<RectTransform>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PickUpObject obj))
        {
            obj.FloatItemInformation(itemSpot, obj);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PickUpObject obj))
        {
            obj.DisableItemInformation(obj);
        }
    }
}