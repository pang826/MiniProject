using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class PickUpObject : MonoBehaviour
{
    private Dictionary<PickUpObject, GameObject> items = new Dictionary<PickUpObject, GameObject>();

    public virtual void FloatItemInformation(GameObject itemSpot, GameObject prefab, PickUpObject obj, List<PickUpObject> itemList)
    {
        GameObject itemImage = Instantiate(prefab, itemSpot.transform);
        TextMeshProUGUI tmp = itemImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        itemList.Add(obj);
        tmp.text = obj.name;

        if (!items.ContainsKey(obj))
        {
            items[obj] = itemImage;
        }
    }

    public virtual void DisableItemInformation(PickUpObject obj, List<PickUpObject> itemList)
    {
        if (items.TryGetValue(obj, out GameObject itemImage))
        {
            Destroy(itemImage);
            items.Remove(obj);
        }

        if (itemList.Contains(obj))
        {
            itemList.Remove(obj);
        }
    }
}
