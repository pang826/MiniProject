using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class PickUpObject : MonoBehaviour
{
    private Dictionary<PickUpObject, GameObject> items = new Dictionary<PickUpObject, GameObject>();
    public Sprite itemImage;
    [SerializeField] private GameObject prefab;
    private Transform player;
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        prefab = Resources.Load<GameObject>("ItemImage");
    }

    public virtual void FloatItemInformation(RectTransform itemSpot, PickUpObject obj)
    {
        CreateObj(itemSpot, obj);
    }

    public virtual void ItemToInventory(RectTransform inventory, PickUpObject obj)
    {
        CreateObj(inventory, obj);

        inventory.GetComponent<Inventory>().InventoryLinkedList.AddLast(obj);

        
        obj.gameObject.SetActive(false);
    }

    public virtual void InventoryToItem(RectTransform itemSpot, PickUpObject obj, RectTransform inventory)
    {
        CreateObj(itemSpot, obj);

        inventory.GetComponent<Inventory>().InventoryLinkedList.Remove(obj);
        if(obj.gameObject.activeSelf == false)
        {
            obj.gameObject.SetActive(true);
            obj.gameObject.transform.position = player.position;
        }
    }

    private void CreateObj(RectTransform rectTransform, PickUpObject obj)
    {
        DisableItemInformation(obj);
        // 아이템 정보 프리팹 생성
        if (!items.ContainsKey(obj))
        {
            GameObject itemImage = Instantiate(prefab, rectTransform.transform.GetChild(0).GetChild(0));
            // 프리팹의 PickupObj 변수에 해당 오브젝트의 PickupObj 할당
            itemImage.GetComponent<DragAndDropUI>().obj = obj;

            Image image = itemImage.GetComponent<Image>();
            image.sprite = obj.itemImage;
            TextMeshProUGUI tmp = itemImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            tmp.text = obj.name;

            items[obj] = itemImage;
        }
    }
    public virtual void DisableItemInformation(PickUpObject obj)
    {
        Debug.Log("삭제");
        if (items.TryGetValue(obj, out GameObject itemImage))
        {
            Destroy(itemImage);
            items.Remove(obj);
        }
    }
}
