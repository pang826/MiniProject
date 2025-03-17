using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropUI : MonoBehaviour, 
    IPointerDownHandler, IPointerUpHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public RectTransform ItemSpot;
    public RectTransform Inventory;
    [SerializeField] Image colorControlImage;
    public Image itemImage;
    Vector3 startPos;

    public PickUpObject obj;
    private void Awake()
    {
        itemImage = GetComponent<Image>();
    }
    private void OnEnable()
    {
        startPos = transform.gameObject.GetComponent<RectTransform>().anchoredPosition;
        startPos.x = 0;
        ItemSpot = GameObject.FindGameObjectWithTag("ItemSpot").GetComponent<RectTransform>();
        Inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<RectTransform>();
        
    }
    public void OnPointerDown(PointerEventData data)
    {
        colorControlImage.color = new Color(colorControlImage.color.r, colorControlImage.color.g, colorControlImage.color.b, 0.2f);
    }

    public void OnPointerUp(PointerEventData data) 
    {
        colorControlImage.color = new Color(colorControlImage.color.r, colorControlImage.color.g, colorControlImage.color.b, 0f);
    }

    public void OnBeginDrag(PointerEventData data)
    {
        startPos = transform.position;
    }

    public void OnEndDrag(PointerEventData data)
    {
        Debug.Log(startPos);
        Vector2 mousePosition = Input.mousePosition;
        
        // 아이템스팟에서 아이템스팟
        if (RectTransformUtility.RectangleContainsScreenPoint(ItemSpot, mousePosition) 
            && RectTransformUtility.RectangleContainsScreenPoint(ItemSpot, data.pressPosition))
        {
            Debug.Log("아이템 투 아이템");
            transform.position = startPos;
        }
        // 아이템스팟에서 인벤토리
        else if (RectTransformUtility.RectangleContainsScreenPoint(Inventory, mousePosition) 
            && RectTransformUtility.RectangleContainsScreenPoint(ItemSpot, data.pressPosition))
        {
            obj.ItemToInventory(Inventory, obj);
            Debug.Log("아이템 투 인벤토리");
        }
        // 인벤토리에서 아이템스팟
        else if (RectTransformUtility.RectangleContainsScreenPoint(ItemSpot, mousePosition)
            && RectTransformUtility.RectangleContainsScreenPoint(Inventory, data.pressPosition))
        {
            obj.InventoryToItem(ItemSpot, obj, Inventory);
            Debug.Log("인벤토리 투 아이템");
        }
        // 인벤토리에서 인벤토리
        else if (RectTransformUtility.RectangleContainsScreenPoint(Inventory, mousePosition)
            && RectTransformUtility.RectangleContainsScreenPoint(Inventory, data.pressPosition))
        {
            Debug.Log("인벤토리 투 인벤토리");
            transform.position = startPos;
        }
        colorControlImage.color = new Color(colorControlImage.color.r, colorControlImage.color.g, colorControlImage.color.b, 0f);
    }

    public void OnDrag(PointerEventData data)
    {
        transform.position = data.position;
    }
}
